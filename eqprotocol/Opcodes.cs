﻿namespace OpenEQ.Netcode {

    // All opcodes listed below are for the Underfoot client
    // https://github.com/pjwendy/Server/blob/7fed8fc8c88aca3eca86062d7ef199f2f3160165/utils/patches/patch_UF.conf

    public enum SessionOp : ushort {
        Request = 0x0001, 
        Response = 0x0002, 
        Disconnect = 0x0005, 
        KeepAlive = 0x0006,
        Stats = 0x0007, 
        Ack = 0x0015, 
        OutOfOrder = 0x0011, 
        Single = 0x0009, 
        Fragment = 0x000d, 
        Combined = 0x0003, 
		Bare = 0xFFFF // FAKE
    }

    public enum LoginOp : ushort {
        SessionReady = 0x0001,
        Login = 0x0002,
        ServerListRequest = 0x0004,
        PlayEverquestRequest = 0x000d,
        PlayEverquestResponse = 0x0022,
        ChatMessage = 0x0017,
        LoginAccepted = 0x0018,
        ServerListResponse = 0x0019,
        Poll = 0x0029,
        EnterChat = 0x000f,
        PollResponse = 0x0011
    }

    public enum WorldOp : ushort {
        SendLoginInfo = 0x13da, 
        ApproveWorld = 0x86c7, 
        LogServer = 0x6f79, 
        SendCharInfo = 0x4200, 
        ExpansionInfo = 0x7e4d,
        GuildsList = 0x5b0b,  
        EnterWorld = 0x51b9, 
        PostEnterWorld = 0x5d32, 
        DeleteCharacter = 0x5ca5, 
        CharacterCreateRequest = 0x53a3, 
        CharacterCreate = 0x1b85, 
        RandomNameGenerator = 0x647a, 
        ApproveName = 0x4f1f, 
        MessageOfTheDay = 0x7629, 
        ZoneServerInfo = 0x1190, 
        WorldComplete = 0x441c, 
        SetChatServer = 0x7d90, 
        SetChatServer2 = 0x158f,
        WorldClientReady = 0x7d05,
        AckPacket = 0x3594
    }

    public enum ZoneOp : ushort {
        AckPacket = 0x3594,
        ZoneEntry = 0x4b61,
        ReqNewZone = 0x4118,
        NewZone = 0x43ac,
        ZoneSpawns = 0x7114,
        PlayerProfile = 0x6022,
        TimeOfDay = 0x6015,
        LevelUpdate = 0x6a99,
        Stamina = 0x3d86,
        RequestClientZoneChange = 0x18ea,
        ZoneChange = 0x6d37,
        SpawnAppearance = 0x3e17,
        ChangeSize = 0x6942,
        TributeUpdate = 0x684c,
        TributeTimer = 0x4895,
        TaskDescription = 0x156c,
        TaskActivity = 0x31f3,
        CompletedTasks = 0x687f,
        Weather = 0x4658,
        SendAATable = 0x6ef9,
        ClearAA = 0x2cd4,
        ClearLeadershipAbilities = 0x7b77,
        UpdateAA = 0x7bf6,
        RespondAA = 0x1fbd,
        ReqClientSpawn = 0x69cd,
        SpawnDoor = 0x6f2b,
        GroundSpawn = 0x5c85,
        SendZonepoints = 0x2370,
        SendAAStats = 0x78b9,
        WorldObjectsSent = 0x7b73,
        BlockedBuffs = 0x05d5,
        RemoveBlockedBuffs = 0x37c1,
        ClearBlockedBuffs = 0x5570,
        SendExpZonein = 0x47e7,
        SendTributes = 0x6bfb,
        TributeInfo = 0x5a67,
        SendGuildTributes = 0x4df0,
        AAExpUpdate = 0x4aa2,
        ExpUpdate = 0x0555,
        HPUpdate = 0x6145,
        ManaChange = 0x569a,
        TGB = 0x42ef,
        SpecialMesg = 0x016c,
        GuildMemberList = 0x51bc,
        GuildMOTD = 0x5658,
        CharInventory = 0x47ae,
        WearChange = 0x0400,
        ClientUpdate = 0x7062,
        ClientReady = 0x6cdc,
        SetServerFilter = 0x2d74,
        GetGuildMOTD = 0x1899,
        GetGuildMOTDReply = 0x4a5c,
        GuildMemberUpdate = 0x0a53,
        GuildInvite = 0x1a58,
        GuildRemove = 0x3c02,
        GuildPeace = 0x2bff,
        SetGuildMOTD = 0x053a,
        GuildList = 0x5b0b,
        GuildWar = 0x5408,
        GuildLeader = 0x0598,
        GuildDelete = 0x3f55,
        GuildInviteAccept = 0x7b64,
        GuildDemote = 0x457d,
        GuildPublicNote = 0x2dbd,
        GuildManageBanker = 0x1e4c,
        GuildBank = 0x0d8a,
        SetGuildRank = 0x4ffe,
        GuildUpdateURLAndChannel = 0x5232,
        GuildStatus = 0x28c8,
        GuildCreate = 0x192d,
        LFGuild = 0x7e23,
        GMServers = 0x6989,
        GMBecomeNPC = 0x56e7,
        GMZoneRequest = 0x3fd2,
        GMZoneRequest2 = 0x538f,
        GMGoto = 0x5ebc,
        GMSearchCorpse = 0x5a81,
        GMHideMe = 0x28ef,
        GMDelCorpse = 0x655c,
        GMApproval = 0x7312,
        GMToggle = 0x097f,
        GMSummon = 0x712b,
        GMEmoteZone = 0x1ac1,
        GMEmoteWorld = 0x2444,
        GMFind = 0x6e27,
        GMKick = 0x0402,
        GMKill = 0x799c,
        GMNameChange = 0x0f48,
        GMLastName = 0x7bfb,
        InspectAnswer = 0x0c2b,
        BeginCast = 0x0d5a,
        ColoredText = 0x71bf,
        ConsentResponse = 0x0e87,
        MemorizeSpell = 0x3887,
        LinkedReuse = 0x1b26,
        SwapSpell = 0x5805,
        CastSpell = 0x50c2,
        Consider = 0x3c2d,
        FormattedMessage = 0x3b52,
        SimpleMessage = 0x1f4d,
        Buff = 0x0d1d,
        Illusion = 0x231f,
        MoneyOnCorpse = 0x4a83,
        RandomReply = 0x6d5d,
        DenyResponse = 0x6129,
        SkillUpdate = 0x7f01,
        GMTrainSkillConfirm = 0x3190,
        RandomReq = 0x139d,
        Death = 0x7f9e,
        Bind_Wound = 0x4b1a,
        GMTraining = 0x51fa,
        GMEndTraining = 0x5479,
        GMTrainSkill = 0x2257,
        Animation = 0x4a61,
        Begging = 0x53f9,
        Consent = 0x6bb9,
        ConsentDeny = 0x4cd1,
        AutoFire = 0x5db5,
        PetCommands = 0x7706,
        PetHoTT = 0x2528,
        DeleteSpell = 0x0698,
        Surname = 0x44ae,
        ClearSurname = 0x6705,
        FaceChange = 0x37a7,
        SenseHeading = 0x1b8a,
        Action = 0x0f14,
        ConsiderCorpse = 0x0a18,
        HideCorpse = 0x2d08,
        CorpseDrag = 0x3331,
        CorpseDrop = 0x2e70,
        Bug = 0x2369,
        Feedback = 0x7705,
        Report = 0x50d0,
        Damage = 0x631a,
        ChannelMessage = 0x2e79,        
        Assist = 0x35b1,
        AssistGroup = 0x194f,
        MoveCoin = 0x6024,
        ZonePlayerToBind = 0x382c,
        KeyRing = 0x5c06,
        WhoAllRequest = 0x177a,
        WhoAllResponse = 0x6ffa,
        FriendsWho = 0x6275,
        ConfirmDelete = 0x3edc,
        Logout = 0x224f,
        Rewind = 0x7d63,
        TargetCommand = 0x756c,
        InspectRequest = 0x7c94,
        Hide = 0x3497,
        Jump = 0x083b,
        Camp = 0x5f85,
        Emote = 0x3164,
        SetRunMode = 0x3d06,
        BankerChange = 0x300a,
        TargetMouse = 0x5f5e,
        MobHealth = 0x15de,
        InitialMobHealth = 0x5cb0,
        TargetHoTT = 0x790c,
        XTargetResponse = 0x6eb5,
        XTargetRequest = 0x4750,
        XTargetAutoAddHaters = 0x1a28,
        XTargetOpen = 0x11ae,
        XTargetOpenResponse = 0x45d3,
        TargetBuffs = 0x3f24,
        BuffCreate = 0x2121,
        BuffRemoveRequest = 0x4065,
        DeleteSpawn = 0x58c5,
        AutoAttack = 0x1df9,
        AutoAttack2 = 0x517b,
        Consume = 0x24c5,
        MoveItem = 0x2641,
        DeleteItem = 0x66e0,
        DeleteCharge = 0x4ca1,
        ItemPacket = 0x7b6e,
        ItemLinkResponse = 0x695c,
        ItemLinkClick = 0x3c66,
        NewSpawn = 0x429b,
        Track = 0x709d,
        TrackTarget = 0x3f49,
        TrackUnknown = 0x03e7,
        ClickDoor = 0x6e97,
        MoveDoor = 0x3154,
        RemoveAllDoors = 0x6215,
        EnvDamage = 0x2730,
        BoardBoat = 0x7554,
        Forage = 0x739b,
        LeaveBoat = 0x7286,
        ControlBoat = 0x7ea8,
        SafeFallSuccess = 0x6df7,
        RezzComplete = 0x30a4,
        RezzRequest = 0x32af,
        RezzAnswer = 0x2d41,
        Shielding = 0x4675,
        RequestDuel = 0x6cfe,
        MobRename = 0x0507,
        AugmentItem = 0x7c87,
        WeaponEquip1 = 0x4572,
        PlayerStateAdd = 0x399b,
        PlayerStateRemove = 0x416b,
        ApplyPoison = 0x5cd3,
        Save = 0x6618,
        TestBuff = 0x3415,
        CustomTitles = 0x6a7e,
        Split = 0x1418,
        YellForHelp = 0x55a8,
        LoadSpellSet = 0x6617,
        Bandolier = 0x510c,
        PotionBelt = 0x0651,
        DuelResponse = 0x41a6,
        DuelResponse2 = 0x6d60,
        SaveOnZoneReq = 0x2913,
        ReadBook = 0x465e,
        Dye = 0x2137,
        InterruptCast = 0x7566,
        AAAction = 0x2bad,
        LeadershipExpToggle = 0x5033,
        LeadershipExpUpdate = 0x074f,
        PurchaseLeadershipAA = 0x5f55,
        UpdateLeadershipAA = 0x77ed,
        MarkNPC = 0x66bf,
        ClearNPCMarks = 0x5c29,
        DoGroupLeadershipAbility = 0x0068,
        GroupLeadershipAAUpdate = 0x167b,
        DelegateAbility = 0x6e58,
        SetGroupTarget = 0x6b9e,
        Charm = 0x1fd5,
        Stun = 0x3d00,
        SendFindableNPCs = 0x6193,
        FindPersonRequest = 0x1e04,
        FindPersonReply = 0x7cae,
        Sound = 0x737a,
        PetBuffWindow = 0x7b87,
        LevelAppearance = 0x1bd4,
        Translocate = 0x3d9c,
        Sacrifice = 0x301b,
        PopupResponse = 0x6d27,
        OnLevelMessage = 0x24cb,
        AugmentInfo = 0x31b1,
        Petition = 0x31d1,
        SomeItemPacketMaybe = 0x2c27,
        PVPStats = 0x5272,
        PVPLeaderBoardRequest = 0x4973,
        PVPLeaderBoardReply = 0x3842,
        PVPLeaderBoardDetailsRequest = 0x6c75,
        PVPLeaderBoardDetailsReply = 0x7fd7,
        RestState = 0x5d24,
        RespawnWindow = 0x107f,
        DisciplineTimer = 0x047c,
        LDoNButton = 0x1031,
        SetStartCity = 0x68f0,
        VoiceMacroIn = 0x1524,
        VoiceMacroOut = 0x1d99,
        VetRewardsAvaliable = 0x0baa,
        VetClaimRequest = 0x34f8,
        VetClaimReply = 0x6a5d,
        CrystalCountUpdate = 0x3fc8,
        DisciplineUpdate = 0x6ed3,
        MobUpdate = 0x4656,
        NPCMoveUpdate = 0x0f3e,
        CameraEffect = 0x6b0e,
        SpellEffect = 0x57a3,
        RemoveNimbusEffect = 0x2c77,
        AltCurrency = 0x659e,
        AltCurrencyMerchantRequest = 0x214c,
        AltCurrencyMerchantReply = 0x4348,
        AltCurrencyPurchase = 0x4ad7,
        AltCurrencySell = 0x14cf,
        AltCurrencySellSelection = 0x322a,
        AltCurrencyReclaim = 0x365d,
        CrystalReclaim = 0x726e,
        CrystalCreate = 0x12f3,
        Untargetable = 0x301d,
        IncreaseStats = 0x4acf,
        Weblink = 0x6840,
        InspectMessageUpdate = 0x7fa1,
        OpenContainer = 0x041a,
        Marquee = 0x3675,
        Fling = 0x51b1,
        FloatListThing = 0x61ba,
        CancelSneakHide = 0x7686,
        DzQuit = 0x1539,
        DzListTimers = 0x21e9,
        DzAddPlayer = 0x3657,
        DzRemovePlayer = 0x054e,
        DzSwapPlayer = 0x4661,
        DzMakeLeader = 0x226f,
        DzPlayerList = 0x74e4,
        DzJoinExpeditionConfirm = 0x3c5e,
        DzJoinExpeditionReply = 0x1154,
        DzExpeditionInfo = 0x1150,
        DzMemberStatus = 0x2d17,
        DzLeaderStatus = 0x2caf,
        DzExpeditionEndsWarning = 0x6ac2,
        DzExpeditionList = 0x70d8,
        DzMemberList = 0x15c4,
        DzCompass = 0x01cb,
        DzChooseZone = 0x65e1,
        SpawnPositionUpdate = 0x4656,
        ManaUpdate = 0x0433,
        EnduranceUpdate = 0x6b76,
        MobManaUpdate = 0x7901,
        MobEnduranceUpdate = 0x1912,
        LootRequest = 0x6ad7,
        EndLootRequest = 0x6546,
        LootItem = 0x5960,
        LootComplete = 0x604d,
        BazaarSearch = 0x550f,
        TraderDelItem = 0x63c8,
        BecomeTrader = 0x0a1d,
        TraderShop = 0x2881,
        Trader = 0x0c08,
        TraderBuy = 0x3672,
        Barter = 0x6db5,
        TradeRequest = 0x7113,
        TradeAcceptClick = 0x064a,
        TradeRequestAck = 0x606a,
        TradeCoins = 0x0149,
        FinishTrade = 0x3ff6,
        CancelTrade = 0x527e,
        TradeMoneyUpdate = 0x2a6d,
        MoneyUpdate = 0xd677,
        TradeBusy = 0x5ed3,
        FinishWindow = 0x3c27,
        FinishWindow2 = 0x6759,
        ItemVerifyRequest = 0x101e,
        ItemVerifyReply = 0x21c7,
        ShopPlayerSell = 0x0b27,
        ShopRequest = 0x442a,
        ShopEnd = 0x3753,
        ShopEndConfirm = 0x4578,
        ShopPlayerBuy = 0x436a,
        ShopDelItem = 0x63c8,
        ClickObject = 0x33e5,
        ClickObjectAction = 0x41b5,
        ClearObject = 0x71d1,
        RecipeDetails = 0x58d9,
        RecipesFavorite = 0x7770,
        RecipesSearch = 0x6948,
        RecipeReply = 0x521c,
        RecipeAutoCombine = 0x0322,
        TradeSkillCombine = 0x4212,
        OpenGuildTributeMaster = 0x5e79,
        OpenTributeMaster = 0x7c24,
        SelectTribute = 0x0c98,
        TributeItem = 0x0b89,
        TributeMoney = 0x314f,
        TributeToggle = 0x6dc3,
        TributePointUpdate = 0x15a7,
        LeaveAdventure = 0x3ed4,
        AdventureFinish = 0x6acc,
        AdventureInfoRequest = 0x3541,
        AdventureInfo = 0x5cea,
        AdventureRequest = 0x2c03,
        AdventureDetails = 0x1d40,
        AdventureData = 0x34f2,
        AdventureUpdate = 0x771f,
        AdventureMerchantRequest = 0x4e22,
        AdventureMerchantResponse = 0x4dd5,
        AdventureMerchantPurchase = 0x7b7f,
        AdventureMerchantSell = 0x179d,
        AdventurePointsUpdate = 0x7537,
        AdventureStatsRequest = 0x4786,
        AdventureStatsReply = 0x38b0,
        AdventureLeaderboardRequest = 0x4cc6,
        AdventureLeaderboardReply = 0x4423,
        GroupDisband = 0x54e8,
        GroupInvite = 0x4f60,
        GroupFollow = 0x7f2b,
        GroupUpdate = 0x5331,
        GroupUpdateB = 0x0786,
        GroupCancelInvite = 0x2736,
        GroupAcknowledge = 0x3e22,
        GroupDelete = 0x58e6,
        GroupFollow2 = 0x6c16,
        GroupInvite2 = 0x5251,
        GroupDisbandYou = 0x0bd0,
        GroupDisbandOther = 0x49f6,
        GroupLeaderChange = 0x0c33,
        GroupRoles = 0x116d,
        GroupMakeLeader = 0x5851,
        GroupMentor = 0x292f,
        InspectBuffs = 0x105b,
        LFGCommand = 0x2c38,
        LFGGetMatchesRequest = 0x28d4,
        LFGGetMatchesResponse = 0x7a16,
        LFPGetMatchesRequest = 0x189e,
        LFPGetMatchesResponse = 0x589f,
        LFPCommand = 0x7429,
        RaidInvite = 0x60b5,
        RaidUpdate = 0x4d8b,
        Taunt = 0x30e2,
        CombatAbility = 0x36f8,
        SenseTraps = 0x7e45,
        PickPocket = 0x5821,
        Disarm = 0x6def,
        Sneak = 0x1d22,
        Fishing = 0x7093,
        InstillDoubt = 0x221a,
        FeignDeath = 0x002b,
        Mend = 0x10a6,
        LDoNOpen = 0x032b,
        LDoNDisarmTraps = 0x1a84,
        LDoNPickLock = 0x0370,
        LDoNInspect = 0x0aaa,
        TaskActivityComplete = 0x5832,
        TaskMemberList = 0x66ba,
        OpenNewTasksWindow = 0x98f6,
        AcceptNewTask = 0x17d5,
        TaskHistoryRequest = 0x547c,
        TaskHistoryReply = 0x4524,
        CancelTask = 0x3bf5,
        NewTitlesAvailable = 0x4b49,
        RequestTitles = 0x4d3e,
        SendTitleList = 0x0d96,
        SetTitle = 0x675c,
        SetTitleReply = 0x75f5,
        MercenaryDataRequest = 0x3015,
        MercenaryDataResponse = 0x0eaa,
        MercenaryHire = 0x099e,
        MercenaryTimer = 0x0cae,
        MercenaryAssign = 0x2538,
        MercenaryUnknown1 = 0x367f,
        MercenaryDataUpdate = 0x57f2,
        MercenaryCommand = 0x50c1,
        MercenarySuspendRequest = 0x3c58,
        MercenarySuspendResponse = 0x4b82,
        MercenaryUnsuspendResponse = 0x5fe3,
        MercenaryDataUpdateRequest = 0x05f1,
        MercenaryDismiss = 0x319a,
        MercenaryTimerRequest = 0x184e,
        PreLogoutReply = 0x711e,
        LogoutReply = 0x3cdc
    }
}
