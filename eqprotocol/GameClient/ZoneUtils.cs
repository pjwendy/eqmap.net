using System;
using System.Collections.Generic;

namespace OpenEQ.Netcode.GameClient
{
    /// <summary>
    /// Utility class for zone name and ID conversions
    /// Provides the canonical EverQuest zone name mappings
    /// </summary>
    public static class ZoneUtils
    {
        private static readonly Dictionary<string, uint> ZoneNameToIdMap = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase)
        {
            {"qeynos", 1}, {"qeynos2", 2}, {"qrg", 3}, {"qeytoqrg", 4}, {"highpass", 5},
            {"highkeep", 6}, {"freportn", 8}, {"freportw", 9}, {"freporte", 10}, {"runnyeye", 11},
            {"qey2hh1", 12}, {"northkarana", 13}, {"southkarana", 14}, {"eastkarana", 15}, {"beholder", 16},
            {"blackburrow", 17}, {"paw", 18}, {"rivervale", 19}, {"kithicor", 20}, {"commons", 21},
            {"ecommons", 22}, {"erudnint", 23}, {"erudnext", 24}, {"nektulos", 25}, {"cshome", 26},
            {"lavastorm", 27}, {"nektropos", 28}, {"halas", 29}, {"everfrost", 30}, {"soldunga", 31},
            {"soldungb", 32}, {"misty", 33}, {"nro", 34}, {"sro", 35}, {"befallen", 36},
            {"oasis", 37}, {"tox", 38}, {"hole", 39}, {"neriaka", 40}, {"neriakb", 41},
            {"neriakc", 42}, {"neriakd", 43}, {"najena", 44}, {"qcat", 45}, {"innothule", 46},
            {"feerrott", 47}, {"cazicthule", 48}, {"oggok", 49}, {"rathemtn", 50}, {"lakerathe", 51},
            {"grobb", 52}, {"aviak", 53}, {"gfaydark", 54}, {"akanon", 55}, {"steamfont", 56},
            {"lfaydark", 57}, {"crushbone", 58}, {"mistmoore", 59}, {"kaladima", 60}, {"felwithea", 61},
            {"felwitheb", 62}, {"unrest", 63}, {"kedge", 64}, {"guktop", 65}, {"gukbottom", 66},
            {"kaladimb", 67}, {"butcher", 68}, {"oot", 69}, {"cauldron", 70}, {"airplane", 71},
            {"fearplane", 72}, {"permafrost", 73}, {"kerraridge", 74}, {"paineel", 75}, {"hateplane", 76},
            {"arena", 77}, {"fieldofbone", 78}, {"warslikswood", 79}, {"soltemple", 80}, {"droga", 81},
            {"cabwest", 82}, {"swampofnohope", 83}, {"firiona", 84}, {"lakeofillomen", 85}, {"dreadlands", 86},
            {"burningwood", 87}, {"kaesora", 88}, {"sebilis", 89}, {"citymist", 90}, {"skyfire", 91},
            {"frontiermtns", 92}, {"overthere", 93}, {"emeraldjungle", 94}, {"trakanon", 95}, {"timorous", 96},
            {"kurn", 97}, {"erudsxing", 98}, {"stonebrunt", 100}, {"warrens", 101}, {"karnor", 102},
            {"chardok", 103}, {"dalnir", 104}, {"charasis", 105}, {"cabeast", 106}, {"nurga", 107},
            {"veeshan", 108}, {"veksar", 109}, {"iceclad", 110}, {"frozenshadow", 111}, {"velketor", 112},
            {"kael", 113}, {"skyshrine", 114}, {"thurgadina", 115}, {"eastwastes", 116}, {"cobaltscar", 117},
            {"greatdivide", 118}, {"wakening", 119}, {"westwastes", 120}, {"crystal", 121}, {"necropolis", 123},
            {"templeveeshan", 124}, {"sirens", 125}, {"mischiefplane", 126}, {"growthplane", 127}, {"sleeper", 128},
            {"thurgadinb", 129}, {"erudsxing2", 130}, {"shadowhaven", 150}, {"bazaar", 151}, {"nexus", 152},
            {"echo", 153}, {"acrylia", 154}, {"sharvahl", 155}, {"paludal", 156}, {"fungusgrove", 157},
            {"vexthal", 158}, {"sseru", 159}, {"katta", 160}, {"netherbian", 161}, {"ssratemple", 162},
            {"griegsend", 163}, {"thedeep", 164}, {"shadeweaver", 165}, {"hollowshade", 166}, {"grimling", 167},
            {"mseru", 168}, {"letalis", 169}, {"twilight", 170}, {"thegrey", 171}, {"tenebrous", 172},
            {"maiden", 173}, {"dawnshroud", 174}, {"scarlet", 175}, {"umbral", 176}, {"akheva", 179},
            {"arena2", 180}, {"jaggedpine", 181}, {"nedaria", 182}, {"tutorial", 183}, {"load", 184},
            {"load2", 185}, {"hateplaneb", 186}, {"shadowrest", 187}, {"tutoriala", 188}, {"tutorialb", 189},
            {"clz", 190}, {"codecay", 200}, {"pojustice", 201}, {"poknowledge", 202}, {"potranquility", 203},
            {"ponightmare", 204}, {"podisease", 205}, {"poinnovation", 206}, {"potorment", 207}, {"povalor", 208},
            {"bothunder", 209}, {"postorms", 210}, {"hohonora", 211}, {"solrotower", 212}, {"powar", 213},
            {"potactics", 214}, {"poair", 215}, {"powater", 216}, {"pofire", 217}, {"poeartha", 218},
            {"potimea", 219}, {"hohonorb", 220}, {"nightmareb", 221}, {"poearthb", 222}, {"potimeb", 223},
            {"gunthak", 224}, {"dulak", 225}, {"torgiran", 226}, {"nadox", 227}, {"hatesfury", 228},
            {"guka", 229}, {"ruja", 230}, {"taka", 231}, {"mira", 232}, {"mmca", 233},
            {"gukb", 234}, {"rujb", 235}, {"takb", 236}, {"mirb", 237}, {"mmcb", 238},
            {"gukc", 239}, {"rujc", 240}, {"takc", 241}, {"mirc", 242}, {"mmcc", 243},
            {"gukd", 244}, {"rujd", 245}, {"takd", 246}, {"mird", 247}, {"mmcd", 248},
            {"guke", 249}, {"ruje", 250}, {"take", 251}, {"mire", 252}, {"mmce", 253},
            {"gukf", 254}, {"rujf", 255}, {"takf", 256}, {"mirf", 257}, {"mmcf", 258},
            {"gukg", 259}, {"rujg", 260}, {"takg", 261}, {"mirg", 262}, {"mmcg", 263},
            {"gukh", 264}, {"rujh", 265}, {"takh", 266}, {"mirh", 267}, {"mmch", 268},
            {"ruji", 269}, {"taki", 270}, {"miri", 271}, {"mmci", 272}, {"rujj", 273},
            {"takj", 274}, {"mirj", 275}, {"mmcj", 276}, {"chardokb", 277}, {"soldungc", 278},
            {"abysmal", 279}, {"natimbi", 280}, {"qinimi", 281}, {"riwwi", 282}, {"barindu", 283},
            {"ferubi", 284}, {"snpool", 285}, {"snlair", 286}, {"snplant", 287}, {"sncrematory", 288},
            {"tipt", 289}, {"vxed", 290}, {"yxtta", 291}, {"uqua", 292}, {"kodtaz", 293},
            {"ikkinz", 294}, {"qvic", 295}, {"inktuta", 296}, {"txevu", 297}, {"tacvi", 298},
            {"qvicb", 299}, {"wallofslaughter", 300}, {"bloodfields", 301}, {"draniksscar", 302}, {"causeway", 303},
            {"chambersa", 304}, {"chambersb", 305}, {"chambersc", 306}, {"chambersd", 307}, {"chamberse", 308},
            {"chambersf", 309}, {"provinggrounds", 316}, {"anguish", 317}, {"dranikhollowsa", 318}, {"dranikhollowsb", 319},
            {"dranikhollowsc", 320}, {"dranikhollowsd", 321}, {"dranikhollowse", 322}, {"dranikhollowsf", 323}, {"dranikhollowsg", 324},
            {"dranikhollowsh", 325}, {"dranikhollowsi", 326}, {"dranikhollowsj", 327}, {"dranikcatacombsa", 328}, {"dranikcatacombsb", 329},
            {"dranikcatacombsc", 330}, {"draniksewersa", 331}, {"draniksewersb", 332}, {"draniksewersc", 333}, {"riftseekers", 334},
            {"harbingers", 335}, {"dranik", 336}, {"broodlands", 337}, {"stillmoona", 338}, {"stillmoonb", 339},
            {"thundercrest", 340}, {"delvea", 341}, {"delveb", 342}, {"thenest", 343}, {"guildlobby", 344},
            {"guildhall", 345}, {"barter", 346}, {"illsalin", 347}, {"illsalina", 348}, {"illsalinb", 349},
            {"illsalinc", 350}, {"dreadspire", 351}, {"drachnidhive", 354}, {"drachnidhivea", 355}, {"drachnidhiveb", 356},
            {"drachnidhivec", 357}, {"westkorlach", 358}, {"westkorlacha", 359}, {"westkorlachb", 360}, {"westkorlachc", 361},
            {"eastkorlach", 362}, {"eastkorlacha", 363}, {"shadowspine", 364}, {"corathus", 365}, {"corathusa", 366},
            {"corathusb", 367}, {"nektulosa", 368}, {"arcstone", 369}, {"relic", 370}, {"skylance", 371},
            {"devastation", 372}, {"devastationa", 373}, {"rage", 374}, {"ragea", 375}, {"takishruins", 376},
            {"takishruinsa", 377}, {"elddar", 378}, {"elddara", 379}, {"theater", 380}, {"theatera", 381},
            {"freeporteast", 382}, {"freeportwest", 383}, {"freeportsewers", 384}, {"freeportacademy", 385}, {"freeporttemple", 386},
            {"freeportmilitia", 387}, {"freeportarena", 388}, {"freeportcityhall", 389}, {"freeporttheater", 390}, {"freeporthall", 391},
            {"northro", 392}, {"southro", 393}, {"crescent", 394}, {"moors", 395}, {"stonehive", 396},
            {"mesa", 397}, {"roost", 398}, {"steppes", 399}, {"icefall", 400}, {"valdeholm", 401},
            {"frostcrypt", 402}, {"sunderock", 403}, {"vergalid", 404}, {"direwind", 405}, {"ashengate", 406},
            {"highpasshold", 407}, {"commonlands", 408}, {"oceanoftears", 409}, {"kithforest", 410}, {"befallenb", 411},
            {"highpasskeep", 412}, {"innothuleb", 413}, {"toxxulia", 414}, {"mistythicket", 415}, {"kattacastrum", 416},
            {"thalassius", 417}, {"atiiki", 418}, {"zhisza", 419}, {"silyssar", 420}, {"solteris", 421},
            {"barren", 422}, {"buriedsea", 423}, {"jardelshook", 424}, {"monkeyrock", 425}, {"suncrest", 426},
            {"deadbone", 427}, {"blacksail", 428}, {"maidensgrave", 429}, {"redfeather", 430}, {"shipmvp", 431},
            {"shipmvu", 432}, {"shippvu", 433}, {"shipuvu", 434}, {"shipmvm", 435}, {"mechanotus", 436},
            {"mansion", 437}, {"steamfactory", 438}, {"shipworkshop", 439}, {"gyrospireb", 440}, {"gyrospirez", 441},
            {"dragonscale", 442}, {"lopingplains", 443}, {"hillsofshade", 444}, {"bloodmoon", 445}, {"crystallos", 446},
            {"guardian", 447}, {"steamfontmts", 448}, {"cryptofshade", 449}, {"dragonscaleb", 451}, {"oldfieldofbone", 452},
            {"oldkaesoraa", 453}, {"oldkaesorab", 454}, {"oldkurn", 455}, {"oldkithicor", 456}, {"oldcommons", 457},
            {"oldhighpass", 458}, {"thevoida", 459}, {"thevoidb", 460}, {"thevoidc", 461}, {"thevoidd", 462},
            {"thevoide", 463}, {"thevoidf", 464}, {"thevoidg", 465}, {"oceangreenhills", 466}, {"oceangreenvillag", 467},
            {"oldblackburrow", 468}, {"bertoxtemple", 469}, {"discord", 470}, {"discordtower", 471}, {"oldbloodfield", 472},
            {"precipiceofwar", 473}, {"olddranik", 474}, {"toskirakk", 475}, {"korascian", 476}, {"rathechamber", 477},
            {"arttest", 996}, {"fhalls", 998}, {"apprentice", 999}
        };

        private static readonly Dictionary<uint, string> ZoneIdToNameMap = new Dictionary<uint, string>
        {
            {1, "qeynos"}, {2, "qeynos2"}, {3, "qrg"}, {4, "qeytoqrg"}, {5, "highpass"},
            {6, "highkeep"}, {8, "freportn"}, {9, "freportw"}, {10, "freporte"}, {11, "runnyeye"},
            {12, "qey2hh1"}, {13, "northkarana"}, {14, "southkarana"}, {15, "eastkarana"}, {16, "beholder"},
            {17, "blackburrow"}, {18, "paw"}, {19, "rivervale"}, {20, "kithicor"}, {21, "commons"},
            {22, "ecommons"}, {23, "erudnint"}, {24, "erudnext"}, {25, "nektulos"}, {26, "cshome"},
            {27, "lavastorm"}, {28, "nektropos"}, {29, "halas"}, {30, "everfrost"}, {31, "soldunga"},
            {32, "soldungb"}, {33, "misty"}, {34, "nro"}, {35, "sro"}, {36, "befallen"},
            {37, "oasis"}, {38, "tox"}, {39, "hole"}, {40, "neriaka"}, {41, "neriakb"},
            {42, "neriakc"}, {43, "neriakd"}, {44, "najena"}, {45, "qcat"}, {46, "innothule"},
            {47, "feerrott"}, {48, "cazicthule"}, {49, "oggok"}, {50, "rathemtn"}, {51, "lakerathe"},
            {52, "grobb"}, {53, "aviak"}, {54, "gfaydark"}, {55, "akanon"}, {56, "steamfont"},
            {57, "lfaydark"}, {58, "crushbone"}, {59, "mistmoore"}, {60, "kaladima"}, {61, "felwithea"},
            {62, "felwitheb"}, {63, "unrest"}, {64, "kedge"}, {65, "guktop"}, {66, "gukbottom"},
            {67, "kaladimb"}, {68, "butcher"}, {69, "oot"}, {70, "cauldron"}, {71, "airplane"},
            {72, "fearplane"}, {73, "permafrost"}, {74, "kerraridge"}, {75, "paineel"}, {76, "hateplane"},
            {77, "arena"}, {78, "fieldofbone"}, {79, "warslikswood"}, {80, "soltemple"}, {81, "droga"},
            {82, "cabwest"}, {83, "swampofnohope"}, {84, "firiona"}, {85, "lakeofillomen"}, {86, "dreadlands"},
            {87, "burningwood"}, {88, "kaesora"}, {89, "sebilis"}, {90, "citymist"}, {91, "skyfire"},
            {92, "frontiermtns"}, {93, "overthere"}, {94, "emeraldjungle"}, {95, "trakanon"}, {96, "timorous"},
            {97, "kurn"}, {98, "erudsxing"}, {100, "stonebrunt"}, {101, "warrens"}, {102, "karnor"},
            {103, "chardok"}, {104, "dalnir"}, {105, "charasis"}, {106, "cabeast"}, {107, "nurga"},
            {108, "veeshan"}, {109, "veksar"}, {110, "iceclad"}, {111, "frozenshadow"}, {112, "velketor"},
            {113, "kael"}, {114, "skyshrine"}, {115, "thurgadina"}, {116, "eastwastes"}, {117, "cobaltscar"},
            {118, "greatdivide"}, {119, "wakening"}, {120, "westwastes"}, {121, "crystal"}, {123, "necropolis"},
            {124, "templeveeshan"}, {125, "sirens"}, {126, "mischiefplane"}, {127, "growthplane"}, {128, "sleeper"},
            {129, "thurgadinb"}, {130, "erudsxing2"}, {150, "shadowhaven"}, {151, "bazaar"}, {152, "nexus"},
            {153, "echo"}, {154, "acrylia"}, {155, "sharvahl"}, {156, "paludal"}, {157, "fungusgrove"},
            {158, "vexthal"}, {159, "sseru"}, {160, "katta"}, {161, "netherbian"}, {162, "ssratemple"},
            {163, "griegsend"}, {164, "thedeep"}, {165, "shadeweaver"}, {166, "hollowshade"}, {167, "grimling"},
            {168, "mseru"}, {169, "letalis"}, {170, "twilight"}, {171, "thegrey"}, {172, "tenebrous"},
            {173, "maiden"}, {174, "dawnshroud"}, {175, "scarlet"}, {176, "umbral"}, {179, "akheva"},
            {180, "arena2"}, {181, "jaggedpine"}, {182, "nedaria"}, {183, "tutorial"}, {184, "load"},
            {185, "load2"}, {186, "hateplaneb"}, {187, "shadowrest"}, {188, "tutoriala"}, {189, "tutorialb"},
            {190, "clz"}, {200, "codecay"}, {201, "pojustice"}, {202, "poknowledge"}, {203, "potranquility"},
            {204, "ponightmare"}, {205, "podisease"}, {206, "poinnovation"}, {207, "potorment"}, {208, "povalor"},
            {209, "bothunder"}, {210, "postorms"}, {211, "hohonora"}, {212, "solrotower"}, {213, "powar"},
            {214, "potactics"}, {215, "poair"}, {216, "powater"}, {217, "pofire"}, {218, "poeartha"},
            {219, "potimea"}, {220, "hohonorb"}, {221, "nightmareb"}, {222, "poearthb"}, {223, "potimeb"},
            {224, "gunthak"}, {225, "dulak"}, {226, "torgiran"}, {227, "nadox"}, {228, "hatesfury"},
            {229, "guka"}, {230, "ruja"}, {231, "taka"}, {232, "mira"}, {233, "mmca"},
            {234, "gukb"}, {235, "rujb"}, {236, "takb"}, {237, "mirb"}, {238, "mmcb"},
            {239, "gukc"}, {240, "rujc"}, {241, "takc"}, {242, "mirc"}, {243, "mmcc"},
            {244, "gukd"}, {245, "rujd"}, {246, "takd"}, {247, "mird"}, {248, "mmcd"},
            {249, "guke"}, {250, "ruje"}, {251, "take"}, {252, "mire"}, {253, "mmce"},
            {254, "gukf"}, {255, "rujf"}, {256, "takf"}, {257, "mirf"}, {258, "mmcf"},
            {259, "gukg"}, {260, "rujg"}, {261, "takg"}, {262, "mirg"}, {263, "mmcg"},
            {264, "gukh"}, {265, "rujh"}, {266, "takh"}, {267, "mirh"}, {268, "mmch"},
            {269, "ruji"}, {270, "taki"}, {271, "miri"}, {272, "mmci"}, {273, "rujj"},
            {274, "takj"}, {275, "mirj"}, {276, "mmcj"}, {277, "chardokb"}, {278, "soldungc"},
            {279, "abysmal"}, {280, "natimbi"}, {281, "qinimi"}, {282, "riwwi"}, {283, "barindu"},
            {284, "ferubi"}, {285, "snpool"}, {286, "snlair"}, {287, "snplant"}, {288, "sncrematory"},
            {289, "tipt"}, {290, "vxed"}, {291, "yxtta"}, {292, "uqua"}, {293, "kodtaz"},
            {294, "ikkinz"}, {295, "qvic"}, {296, "inktuta"}, {297, "txevu"}, {298, "tacvi"},
            {299, "qvicb"}, {300, "wallofslaughter"}, {301, "bloodfields"}, {302, "draniksscar"}, {303, "causeway"},
            {304, "chambersa"}, {305, "chambersb"}, {306, "chambersc"}, {307, "chambersd"}, {308, "chamberse"},
            {309, "chambersf"}, {316, "provinggrounds"}, {317, "anguish"}, {318, "dranikhollowsa"}, {319, "dranikhollowsb"},
            {320, "dranikhollowsc"}, {321, "dranikhollowsd"}, {322, "dranikhollowse"}, {323, "dranikhollowsf"}, {324, "dranikhollowsg"},
            {325, "dranikhollowsh"}, {326, "dranikhollowsi"}, {327, "dranikhollowsj"}, {328, "dranikcatacombsa"}, {329, "dranikcatacombsb"},
            {330, "dranikcatacombsc"}, {331, "draniksewersa"}, {332, "draniksewersb"}, {333, "draniksewersc"}, {334, "riftseekers"},
            {335, "harbingers"}, {336, "dranik"}, {337, "broodlands"}, {338, "stillmoona"}, {339, "stillmoonb"},
            {340, "thundercrest"}, {341, "delvea"}, {342, "delveb"}, {343, "thenest"}, {344, "guildlobby"},
            {345, "guildhall"}, {346, "barter"}, {347, "illsalin"}, {348, "illsalina"}, {349, "illsalinb"},
            {350, "illsalinc"}, {351, "dreadspire"}, {354, "drachnidhive"}, {355, "drachnidhivea"}, {356, "drachnidhiveb"},
            {357, "drachnidhivec"}, {358, "westkorlach"}, {359, "westkorlacha"}, {360, "westkorlachb"}, {361, "westkorlachc"},
            {362, "eastkorlach"}, {363, "eastkorlacha"}, {364, "shadowspine"}, {365, "corathus"}, {366, "corathusa"},
            {367, "corathusb"}, {368, "nektulosa"}, {369, "arcstone"}, {370, "relic"}, {371, "skylance"},
            {372, "devastation"}, {373, "devastationa"}, {374, "rage"}, {375, "ragea"}, {376, "takishruins"},
            {377, "takishruinsa"}, {378, "elddar"}, {379, "elddara"}, {380, "theater"}, {381, "theatera"},
            {382, "freeporteast"}, {383, "freeportwest"}, {384, "freeportsewers"}, {385, "freeportacademy"}, {386, "freeporttemple"},
            {387, "freeportmilitia"}, {388, "freeportarena"}, {389, "freeportcityhall"}, {390, "freeporttheater"}, {391, "freeporthall"},
            {392, "northro"}, {393, "southro"}, {394, "crescent"}, {395, "moors"}, {396, "stonehive"},
            {397, "mesa"}, {398, "roost"}, {399, "steppes"}, {400, "icefall"}, {401, "valdeholm"},
            {402, "frostcrypt"}, {403, "sunderock"}, {404, "vergalid"}, {405, "direwind"}, {406, "ashengate"},
            {407, "highpasshold"}, {408, "commonlands"}, {409, "oceanoftears"}, {410, "kithforest"}, {411, "befallenb"},
            {412, "highpasskeep"}, {413, "innothuleb"}, {414, "toxxulia"}, {415, "mistythicket"}, {416, "kattacastrum"},
            {417, "thalassius"}, {418, "atiiki"}, {419, "zhisza"}, {420, "silyssar"}, {421, "solteris"},
            {422, "barren"}, {423, "buriedsea"}, {424, "jardelshook"}, {425, "monkeyrock"}, {426, "suncrest"},
            {427, "deadbone"}, {428, "blacksail"}, {429, "maidensgrave"}, {430, "redfeather"}, {431, "shipmvp"},
            {432, "shipmvu"}, {433, "shippvu"}, {434, "shipuvu"}, {435, "shipmvm"}, {436, "mechanotus"},
            {437, "mansion"}, {438, "steamfactory"}, {439, "shipworkshop"}, {440, "gyrospireb"}, {441, "gyrospirez"},
            {442, "dragonscale"}, {443, "lopingplains"}, {444, "hillsofshade"}, {445, "bloodmoon"}, {446, "crystallos"},
            {447, "guardian"}, {448, "steamfontmts"}, {449, "cryptofshade"}, {451, "dragonscaleb"}, {452, "oldfieldofbone"},
            {453, "oldkaesoraa"}, {454, "oldkaesorab"}, {455, "oldkurn"}, {456, "oldkithicor"}, {457, "oldcommons"},
            {458, "oldhighpass"}, {459, "thevoida"}, {460, "thevoidb"}, {461, "thevoidc"}, {462, "thevoidd"},
            {463, "thevoide"}, {464, "thevoidf"}, {465, "thevoidg"}, {466, "oceangreenhills"}, {467, "oceangreenvillag"},
            {468, "oldblackburrow"}, {469, "bertoxtemple"}, {470, "discord"}, {471, "discordtower"}, {472, "oldbloodfield"},
            {473, "precipiceofwar"}, {474, "olddranik"}, {475, "toskirakk"}, {476, "korascian"}, {477, "rathechamber"},
            {996, "arttest"}, {998, "fhalls"}, {999, "apprentice"}
        };

        /// <summary>
        /// Convert zone name to zone ID
        /// </summary>
        /// <param name="zoneName">Zone short name (e.g., "arena", "qeynos")</param>
        /// <returns>Zone ID or 0 if not found</returns>
        public static uint ZoneNameToNumber(string zoneName)
        {
            if (string.IsNullOrEmpty(zoneName))
                return 0;

            return ZoneNameToIdMap.TryGetValue(zoneName, out uint zoneId) ? zoneId : 0;
        }

        /// <summary>
        /// Convert zone ID to zone name
        /// </summary>
        /// <param name="zoneNumber">Zone ID</param>
        /// <returns>Zone short name or "UNKNOWNZONE" if not found</returns>
        public static string ZoneNumberToName(uint zoneNumber)
        {
            return ZoneIdToNameMap.TryGetValue(zoneNumber, out string zoneName) ? zoneName : "UNKNOWNZONE";
        }

        /// <summary>
        /// Check if a zone ID exists in the mapping
        /// </summary>
        /// <param name="zoneNumber">Zone ID to check</param>
        /// <returns>True if zone ID is valid</returns>
        public static bool IsValidZoneId(uint zoneNumber)
        {
            return ZoneIdToNameMap.ContainsKey(zoneNumber);
        }

        /// <summary>
        /// Check if a zone name exists in the mapping
        /// </summary>
        /// <param name="zoneName">Zone name to check</param>
        /// <returns>True if zone name is valid</returns>
        public static bool IsValidZoneName(string zoneName)
        {
            return !string.IsNullOrEmpty(zoneName) && ZoneNameToIdMap.ContainsKey(zoneName);
        }

        /// <summary>
        /// Get all valid zone names
        /// </summary>
        /// <returns>Collection of all zone names</returns>
        public static IEnumerable<string> GetAllZoneNames()
        {
            return ZoneNameToIdMap.Keys;
        }

        /// <summary>
        /// Get all valid zone IDs
        /// </summary>
        /// <returns>Collection of all zone IDs</returns>
        public static IEnumerable<uint> GetAllZoneIds()
        {
            return ZoneIdToNameMap.Keys;
        }

        /// <summary>
        /// Get the total number of known zones
        /// </summary>
        /// <returns>Number of zones in the mapping</returns>
        public static int ZoneCount => ZoneIdToNameMap.Count;
    }
}