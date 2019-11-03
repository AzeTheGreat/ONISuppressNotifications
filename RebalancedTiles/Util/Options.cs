﻿using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using TUNING;

namespace RebalancedTiles
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Options
    {
        public class GenericOptions
        {
            [JsonProperty]
            public bool IsTweaked { get; set; }
            [JsonProperty]
            public int Decor { get; set; }
            [JsonProperty]
            public int DecorRadius { get; set; }
            [JsonProperty]
            public float MovementSpeed { get; set; }
            [JsonProperty]
            public float StrengthMultiplier { get; set; }
        }

        public partial class CarpetTileOptions : GenericOptions
        {
            [JsonProperty]
            public bool IsNotWall { get; set; }
            [JsonProperty]
            public bool IsCombustible { get; set; }
            [JsonProperty]
            public float CombustTemp { get; set; }
            [JsonProperty]
            public int ReedFiberCount { get; set; }
        }
        [Option("Carpet: Not a Wall", "When true, modifies the carpet's decor distribution so that it only works as a floor.")]
        public bool _IsCarpetNotWall { get { return CarpetTile.IsNotWall; } set { CarpetTile.IsNotWall = value; } }
        [Option("Carpet: Combusts", "When true, carpet tiles will burn into normal tiles at high temperatures.")]
        public bool _IsCarpetCombustible { get { return CarpetTile.IsCombustible; } set { CarpetTile.IsCombustible = value; } }

        [Option("Meshed Tiles: Reduce Sunlight", "When true, sunlight's strength will be reduced when moving through airflow and mesh tiles.")]
        [JsonProperty]
        public bool DoMeshedTilesReduceSunlight { get; set; }
        [JsonProperty]
        public float MeshedTilesSunlightReduction { get; set; }

        public Options()
        {
            Tile = new TileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER0.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_2,
                StrengthMultiplier = 1.5f
            };
            BunkerTile = new BunkerTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.PENALTY.TIER0.amount,
                DecorRadius = BUILDINGS.DECOR.PENALTY.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.NEUTRAL,
                StrengthMultiplier = 10f
            };
            GasPermeableMembrane = new GasPermeableMembraneOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.PENALTY.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.PENALTY.TIER1.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.NEUTRAL,
                StrengthMultiplier = 1.5f
            };
            GlassTile = new GlassTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_3,
                StrengthMultiplier = 0.5f
            };
            CarpetTile = new CarpetTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER2.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER2.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.PENALTY_1,
                StrengthMultiplier = 1f,
                IsNotWall = true,
                IsCombustible = true,
                CombustTemp = BUILDINGS.OVERHEAT_TEMPERATURES.LOW_2,
                ReedFiberCount = 1
            };
            MetalTile = new MetalTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.BONUS.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.BONUS.TIER0.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.BONUS_3,
                StrengthMultiplier = 2f
            };
            MeshTile = new MeshTileOptions
            {
                IsTweaked = true,
                Decor = BUILDINGS.DECOR.PENALTY.TIER1.amount,
                DecorRadius = BUILDINGS.DECOR.PENALTY.TIER1.radius,
                MovementSpeed = DUPLICANTSTATS.MOVEMENT.PENALTY_1,
                StrengthMultiplier = 1.5f
            };

            DoMeshedTilesReduceSunlight = true;
            MeshedTilesSunlightReduction = 25;
        }

        private static Options _options;
        public static Options Opts {
            get
            {
                if(_options == null)
                    _options = POptions.ReadSettings<Options>() ?? new Options();

                return _options;
            }
            set { _options = value; } }

        public static void OnLoad()
        {
            PUtil.LogModInit();
            POptions.RegisterOptions(typeof(Options));
        }
    }
}
