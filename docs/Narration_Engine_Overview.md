# Narration Engine – Project Overview

## 🎉 Major Milestone Achieved – September 2025  
**Proof of Concept build and validated with prototype logs!** We have a prompt based generator using a very short snippet of fake game logs.

---

## Vision Statement
Create a **narration engine** that converts raw game event logs into engaging, stylistically controlled text. By abstracting low-level telemetry into higher-level stories, the system delivers compelling recaps, commentary, and lore-driven narratives for players, analysts, and spectators.

---

## Core Concept
**Intelligent Storyteller System** – A pipeline that ingests event logs, organizes them into narrative structures, applies contextual and stylistic constraints, and generates polished text in the desired voice, tone, and length.

---

## Primary Objectives

### 1. **Event Processing & Abstraction**
- **Schema Definition**: Canonical JSON structure for cross-game compatibility.  
- **Noise Filtering**: Remove irrelevant micro-events.  
- **State Tracking**: Maintain world state, player stats, and NPC context.  
- **Highlight Detection**: Identify climaxes, causal chains, and turning points.  

### 2. **Narrative Organization**
- **Temporal Clustering**: Group related events into beats, paragraphs, and chapters.  
- **Discourse Planning**: Structure summaries around setup, escalation, climax, and resolution.  
- **POV & Tense Control**: Consistent perspective across output (omniscient, first-person, etc.).  
- **Multi-Persona Voices**: Esports caster, bard, lore chronicler, grim storyteller.  

### 3. **Narration Generation**
- **LLM Integration**: Constraint-augmented or RAG-powered text generation.  
- **Style Constraints**: Enforce lore rules, terminology, and tone.  
- **Custom Parameters**: Length, detail level, reading difficulty.  
- **Faithfulness Validation**: Trace every claim back to underlying events.  

### 4. **Delivery & Outputs**
- **Narrative Formats**: Markdown/HTML stories, JSON chapter structures, SRT/VTT captions.  
- **Multimedia Support**: TTS voiceovers, highlight reels, interactive timelines.  
- **Live & Post-game**: Support both real-time commentary and end-of-session recaps.  
- **APIs for Integration**: Outputs consumable by dashboards, bots, or streaming overlays.  

---

## Technical Architecture Vision

### Processing Layer
- **Ingestion**: Event adapters for various game engines.  
- **Graph Builder**: Multilayer temporal network representation.  
- **Salience Engine**: Highlight detection, clustering, and causal linking.  
- **State Store**: Tracks lore, NPCs, items, and world constraints.  

### Narrative Layer
- **Discourse Planner**: Event grouping into narrative structure.  
- **Constraint Manager**: Applies world bible, tone rules, safety filters.  
- **Narration Engine**: LLM with constrained decoding & RAG retrieval.  
- **Validation Module**: Style, faithfulness, and coherence checks.  

### Delivery Layer
- **Narrative Outputs**: Markdown, JSON, captions, TL;DR summaries.  
- **Voice/Media Integration**: TTS synthesis, caption overlays.  
- **Interactive Dashboards**: Story timelines, event citations, chapter navigation.  
- **API Gateway**: External integrations for streaming/analytics tools.  

---

## Use Cases & Scenarios

### Game Recaps
- **Dungeon Crawl Recaps**: Dramatic summaries of RPG runs (e.g., Path of Exile).  
- **Esports Commentary**: Highlight-driven match reports for competitive FPS/MOBA games.  
- **Quest Journals**: Diaries auto-generated for characters or factions.  
- **Bug Tracking**: Generate summaries of playthroughs where something happens that shouldn't.

### Analytics & Coaching
- **Match Reports**: Post-game analysis with evidence-linked highlights.  
- **Training Feedback**: Identifying mistakes and strategic turning points.  
- **Content Usage Analytics**: Event-to-narrative coverage for game design teams.  

### Community & Streaming
- **Streamer Tools**: Auto-narration overlays or recap reels.  
- **Discord/Slack Bots**: Daily reports or live commentary feeds.  
- **Fan Content**: Shareable, lore-friendly battle stories.  

---

## Success Metrics

### Technical Goals
- **Faithfulness**: 95%+ narrative claims backed by event IDs.  
- **Performance**: Narration generated within seconds post-match.  
- **Scalability**: Support logs from small RPG runs to 100k+ event esports matches.  
- **Extensibility**: Schema works across multiple genres.  

### Narrative Goals
- **Readability**: Outputs flow as natural stories, not logs.  
- **Style Adherence**: Narratives align with chosen tone/style guide.  
- **Highlight Coverage**: Key moments always captured.  
- **Audience Impact**: Positive reception by both casual readers and analysts.  

---

## Development Phases

### Phase 1: Foundation ✅ MILESTONE ACHIEVED!
- Defined JSON schema for event ingestion.  
- Designed pipeline structure (filtering, clustering, generation).  
- Draft prototypes of narrative generation in dark tone.  
- Implementing faithfulness validator.  
- Building event → narrative mapping database.  

### Phase 2: Narrative Intelligence
- Advanced clustering + discourse planner.  
- POV & persona switching.  
- Constraint-driven generation with RAG.  
- Style guide + lore bible integration.  

### Phase 3: Outputs & Integrations
- Multi-format export (Markdown, JSON, SRT).  
- TTS voiceover module.  
- API for stream overlays and bots.  
- Interactive recap dashboards.  

### Phase 4: Scale & Real-time
- Real-time commentary pipeline.  
- Large log handling (100k+ events).  
- Distributed processing for scalability.  
- Performance benchmarks.  

### Phase 5: Ecosystem
- Plugin system for custom event taxonomies.  
- Community contributions of style guides and world bibles.  
- Advanced analytics integration.  
- Cross-game ontology for universal compatibility.  

---

## Challenges & Considerations

### Technical Challenges
- **Event Diversity**: Different games → different event schemas.  
- **Real-time Latency**: Streaming narration without delay.  
- **Faithfulness**: Prevent hallucinations or contradictions.  
- **Scalability**: Handling logs with hundreds of thousands of events.  

### Narrative Challenges
- **Avoiding Log Dumps**: Summaries must emphasize meaning over movement spam.  
- **Tone Consistency**: Matching requested persona or style without drift.  
- **Narrative Balance**: Covering highlights without over-compression.  
- **Audience Adaptability**: Casual recap vs. analyst-level detail.  

### Operational Challenges
- **Evaluation & Testing**: Benchmarking narrative quality.  
- **Monitoring**: Logging event coverage, style adherence, errors.  
- **Maintenance**: Updating constraints, style guides, and event adapters.  
- **Integration Overhead**: Making APIs usable by streamers/devs.  

---

## Innovation Opportunities

### AI Integration
- **Constraint-augmented LLMs**: Faithful, style-controlled narration.  
- **Reinforcement Learning**: Optimize salience scoring with feedback.  
- **Behavioral Cloning**: Learn narrative rhythms from esports casters.  
- **Multi-agent Planning**: Parallel persona narrators for diverse perspectives.  

### Advanced Features
- **Interactive Storybooks**: Playable battle reports with clickable timelines.  
- **Adaptive Narration**: Tone/length adapted to audience preferences.  
- **Highlight Reels**: Sync narratives with video/audio clips.  
- **Collaborative Storytelling**: Multi-perspective outputs from the same log.  

---

## Long-term Vision
Transform raw game telemetry into **living stories**. Whether for a solo dungeon crawl, a massive esports match, or a persistent RPG world, the narration engine aims to make game experiences more **memorable, shareable, and meaningful**. By combining event processing, narrative planning, and AI generation, it will help bridge the gap between **data and story**, turning gameplay into entertainment and history.  

---

*This document represents the ambitious vision for the Narration Engine. It will be updated as milestones are achieved and the project evolves.*  
