# EQEmu Headless Client — Exploration Bot Design

This document outlines a practical approach to **autonomous exploration** for headless clients on an **EQEmu** server. It focuses on using a **navmesh** for pathfinding, picking goals that maximize map coverage while staying safe, and robustly handling edge cases (stuck, hazards, multi‑zone).

---

## Goals

- Let bots **roam zones autonomously**, discovering and revisiting interesting areas.
- Maintain **per-key ordering** and **replayability** of decisions via event logging (optional).
- Keep the implementation **simple first**, then layer in smarter behavior.

---

## Assumptions

- You can access each zone’s **navigation mesh** (`.nav`) that EQEmu uses for NPC AI (Recast/Detour format).
- Your headless client can **send movement commands** and **observe world state** (position, zone lines, spawn info, etc.).
- You can run **C#** and are open to using **DotRecast** (C# port of Recast/Detour) for on‑client path queries.

> If you don’t have `.nav` files, generate them with the EQEmu map tooling (Recast). Avoid using 2D `.map` polygons for navigation—stick to navmesh polygons for pathing.

---

## Architecture Overview

```
[Zone .nav] ──► [DotRecast (query)] ──► Path (poly refs & straight path)
           │
           ├──► [Explorer Policy] ──► Goal selection (novelty / cost / risk)
           │
           ├──► [Risk Map] ──► Dynamic costs near hazards (spawns, water/lava)
           │
           └──► [POI Index] ──► Zone lines, vendors, hubs, grid waypoints

Path follower ─► Movement packets ─► Server
            └─► Stuck handler / replan
```

### Key pieces
- **NavMesh Query (Detour/DotRecast):** `FindNearestPoly`, `FindPath`, `FindStraightPath`, `Raycast`.
- **Explorer Policy:** Chooses the next goal location from candidates sampled on the navmesh.
- **Risk Map:** Inflates path cost around hazards (hostile spawns, water/lava, known traps).
- **Visit Heatmap:** Tracks how often nav polys/areas were visited to encourage coverage.
- **POI Index:** Zone lines, vendors, banks, quests, and NPC grid paths from DB (optional).

---

## Navigation Substrate

- Use **Recast/Detour** mesh (`.nav`) already present in many EQEmu setups.
- In C#, load with **DotRecast**:
  - `DtNavMesh`, `DtNavMeshQuery` for path queries.
  - Filters for walkability (e.g., avoid water areas if needed).
- Avoid raw 2D line maps for navigation—they lack connectivity/topology.

---

## Data Sources (optional but useful)

- **spawn2 / npc_types**: level, faction, aggro behavior → **risk estimation**.
- **grid / grid_entries**: existing NPC patrols → seed **interesting routes**.
- **Zone lines**: inter‑zone portals → **multi‑zone roaming**.
- **Region tags**: water, lava, city hubs → adjust **costs** or **blacklist**.

---

## Exploration Policies

You can layer these policies or start simple and grow.

### A) Random‑Waypoint on NavMesh (baseline)
1. Sample a random reachable point within a radius (or entire zone).
2. Plan path and walk it.
3. Repeat. Maintain a **visit heatmap** and prefer unvisited areas.

**Pros:** trivial to implement. **Cons:** can feel aimless without POIs.

### B) Frontier Coverage (smarter)
Track a **per‑poly visit score**. Prefer goals that increase coverage:
- Maintain `visit_count[poly]` and/or `last_visited[poly]`.
- Prefer candidates with low visit count / long since last visit.
- Add small decay over time so old areas regain priority.

### C) Points‑of‑Interest + Coverage (rich)
- Precompute **POIs**: zone lines, vendors, banks, quest hubs, dungeons.
- Alternate:
  - “Visit a new/rotating POI”
  - “Cover frontier near current location”

This feels purposeful while still exploring the map.

---

## Goal Scoring

Given candidate goals `g ∈ Candidates`, pick the **max score**:

```
score(g) = wN * Novelty(g) - wC * PathCost(g) - wR * Risk(g) + wP * POIBonus(g)
```

- **Novelty(g):** Higher when path touches many low‑visited polys.
- **PathCost(g):** Path length (or seconds to travel).
- **Risk(g):** Sum of hazard fields along the path (hostile levels, water/lava).
- **POIBonus(g):** Extra weight if goal is a POI on a rotation schedule.

Suggested weights (tune per zone/class):
```
wN = 1.0, wC = 0.2, wR = 1.5, wP = 0.5
```
Candidate generation:
- Uniform random samples on navmesh polys (biased to unvisited regions).
- Add POIs and known safe hubs.
- Keep 64–256 candidates per cycle for stable choices.

---

## Pathfinding & Movement

1. `FindNearestPoly(startPos)` and `FindNearestPoly(goalPos)`.
2. `FindPath(startRef, endRef, startPos, goalPos, filter)` → poly corridor.
3. `FindStraightPath(...)` → waypoints to follow.
4. Move toward each waypoint, checking LOS:
   - If `Raycast` shows obstruction or movement stalls, **replan** from current pos.
5. Use a **local steering tolerance** (stop distance / corner tolerance) to avoid oscillations.

**Filters:** Configure off‑mesh connections (ladders/portals) if present; blacklist undesirable flags (water/lava) depending on your bot’s capability.

---

## Safety & Risk Modeling

Build a **risk field** (higher = worse) and add it to PathCost when scoring:
- **Hostile mobs:** higher risk near mobs that can aggro (level difference, aggro radius heuristic).
- **High‑level clusters:** increase risk proportional to nearby mean level and density.
- **Environment:** water/lava → add constant risk or blacklist entirely.
- **Dynamic:** recently died there? temporarily increase risk (“death scent”).

Implementation sketch:
- For each hazard, add a Gaussian/decay contribution to nearby polys.
- Cache per‑poly risk; update periodically (e.g., every 3–5s).

---

## Stuck Handling

- **No‑progress timeout:** If distance along path hasn’t changed for N seconds → replan.
- **Micro‑shuffle:** Try small random sidesteps, then re‑acquire path.
- **Breadcrumb backtrack:** Keep a short queue of recent safe positions to fall back to.
- **Hard reset:** If repeated failures, pick a fresh goal farther away.

---

## Multi‑Zone Roaming

- Treat **zone lines** as portals (POIs). When within threshold → interact/zone.
- After zoning:
  1. Load the new zone’s `.nav`.
  2. Reset path/query objects.
  3. Keep long‑lived **visit heatmap per zone** (and decay slowly).
- Maintain a **zone visit schedule** to avoid flipping back and forth rapidly.

---

## Behavior Structure

A simple **finite state machine** or **behavior tree** works well:

- **Idle/Scan** → pick candidates, compute scores → **PlanGoal**
- **PlanGoal** → query path; if fails, pick another → **Travel**
- **Travel** → follow straight path; monitor stuck; if blocked → replan
- **Interact** (optional) → POI interaction (vendors, banks)
- **Recover** → unstuck/backtrack
- Loop

---

## Telemetry & Debugging

- Log decisions: selected goal, scores, path length, risk totals.
- Track **coverage**: % of nav polys visited in last X minutes.
- Debug UI (optional): draw navmesh, current path, candidate goals, and risk heatmap.

---

## Minimal C# Sketch (DotRecast)

```csharp
// Pseudo-ish example: DotRecast style
using DotRecast.Recast;
using DotRecast.Detour;

DtNavMesh nav = LoadNavMesh("freporte.nav");      // your loader
var query = new DtNavMeshQuery(nav);

var filter = new DtQueryFilter();
// Configure flags: blacklist water/lava if desired
// filter.SetIncludeFlags(...); filter.SetExcludeFlags(...);

var extents = new float[] { 2f, 4f, 2f }; // search extents for nearest poly

PolyRef startRef = query.FindNearestPoly(startPos, extents, filter, out var sn);
while (true)
{
    // 1) Generate candidate goals (random + POIs), compute scores
    var candidates = GenerateCandidates(query, extents, filter);
    var best = SelectBestGoal(candidates, visitHeatmap, riskField, query, filter);

    // 2) Plan the path
    var endRef = query.FindNearestPoly(best.Pos, extents, filter, out var en);
    var pathRefs = query.FindPath(startRef, endRef, startPos, best.Pos, filter);

    // 3) Get straight path (waypoints)
    var straight = query.FindStraightPath(startPos, best.Pos, pathRefs,
        DtStraightPathOptions.AreaCrossings | DtStraightPathOptions.AllCrossings);

    // 4) Follow
    foreach (var wp in straight)
    {
        if (!HasLineOfSight(query, currentPos, wp.Pos, filter)) { break; } // replan
        MoveToward(wp.Pos); // send movement packets
        if (IsStuck()) { AttemptUnstuck(); break; }
    }

    // 5) Bookkeeping
    MarkVisited(straight, visitHeatmap);
    startPos = CurrentPos();
    startRef = query.FindNearestPoly(startPos, extents, filter, out _);
}
```

> Replace helpers with your actual implementations (sampling, risk, LOS checks, move commands).

---

## Configuration Hints

- **Candidate count:** 64–256 per selection cycle.
- **Cycle period:** 1–3 seconds to rescore/regenerate.
- **Heatmap decay:** halve visits every 10–20 minutes to refresh interest.
- **Aggro radius heuristic:** scale with mob level difference & type.
- **Water/lava:** hard‑exclude unless your class can traverse safely.

---

## Testing Strategy

1. **Offline sandbox:** run pathfinding against static navmesh with mocked spawns.
2. **Playback runs:** record inputs (position, spawns) and replay to test determinism.
3. **Visualization:** overlay paths/candidates/risk on a debug map.
4. **Edge zones:** test dungeons with tight corridors and open fields for sampling quality.

---

## Future Enhancements

- Team exploration (multiple bots) with **territory partitioning**.
- **Curiosity model**: reward novel observations (new NPC types, loot tables).
- Lightweight **RL fine‑tuning** on top of scripted policy.
- Opportunistic interactions (vendor, bank) when passing POIs.

---

## Quick Checklist (MVP)

- [ ] Load `.nav` and run `FindPath`/`FindStraightPath` successfully.
- [ ] Random‑waypoint wander; movement + replan on obstruction.
- [ ] Visit heatmap & simple novelty score.
- [ ] Basic risk field from nearby hostiles + environment.
- [ ] Stuck detection (timeout + micro‑shuffle + backtrack).
- [ ] Zone line detection and handoff to next zone.

---

**That’s it.** Start with random‑waypoint + heatmap and risk, then add POIs and multi‑zone. Once the loop is stable, you can iterate on scoring weights and sampling to fit each zone’s personality.
