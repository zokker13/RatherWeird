using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace RatherWeird.Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HotkeyNameAttribute : Attribute
    {
        public HotkeyNameAttribute(string la)
        {
            HotkeyName = la;
        }

        public string HotkeyName;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HotkeyCategoryAttribute : Attribute
    {
        public HotkeyCategoryAttribute(string category)
        {
            Category = category;
            IsInvisible = false;
        }
        public HotkeyCategoryAttribute(string category, bool isInvisible)
        {
            Category = category;
            IsInvisible = isInvisible;
        }

        public bool IsInvisible;
        public string Category;
    }
    
    public class Ra3HotkeySerializer
    {
        private static PropertyInfo GetCorrectProperty(string key)
        {
            var props = typeof(Ra3HotkeyData).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                HotkeyNameAttribute att =
                    (HotkeyNameAttribute) Attribute.GetCustomAttribute(prop, typeof(HotkeyNameAttribute));

                if (att != null && att.HotkeyName == key)
                {
                    return prop;
                }
            }

            Console.WriteLine($"{key} is so sad");

            return null;
        }

        public static async Task<Ra3HotkeyData> ReadHotkeyFile(string path)
        {
            Ra3HotkeyData data = new Ra3HotkeyData();
            
            foreach (var propertyInfo in typeof(Ra3HotkeyData).GetProperties())
            {
                HotkeyNameAttribute att =
                    (HotkeyNameAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(HotkeyNameAttribute));

                string targetName = att == null
                    ? propertyInfo.Name
                    : att.HotkeyName;
                
            }
            

            using (StreamReader sr = File.OpenText(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = await sr.ReadLineAsync();

                    if (line.Trim().Length > 0)
                    {
                        string[] elements = line.Split('=');
                        string key = elements[0].Trim();
                        string value = elements[1].Trim();

                        PropertyInfo property = GetCorrectProperty(key);


                        // Console.WriteLine($"-> {key}: {value}");


                    }
                }
                
                
            }

            return data;
        }
    }

    public enum Ra3Key
    {
        KEY_NONE = Keys.None,
        KEY_A = Keys.A,
        KEY_B = Keys.B,
        KEY_C = Keys.C,
        KEY_D = Keys.D,
        KEY_E = Keys.E,
        KEY_F = Keys.F,
        KEY_G = Keys.G,
        KEY_H = Keys.H,
        KEY_I = Keys.I,
        KEY_J = Keys.J,
        KEY_K = Keys.K,
        KEY_L = Keys.L,
        KEY_M = Keys.M,
        KEY_N = Keys.N,
        KEY_O = Keys.O,
        KEY_P = Keys.P,
        KEY_Q = Keys.Q,
        KEY_R = Keys.R,
        KEY_S = Keys.S,
        KEY_T = Keys.T,
        KEY_U = Keys.U,
        KEY_V = Keys.V,
        KEY_W = Keys.W,
        KEY_X = Keys.X,
        KEY_Y = Keys.Y,
        KEY_Z = Keys.Z,
        KEY_F1 = Keys.F1,
        KEY_F2 = Keys.F2,
        KEY_F3 = Keys.F3,
        KEY_F4 = Keys.F4,
        KEY_F5 = Keys.F5,
        KEY_F6 = Keys.F6,
        KEY_F7 = Keys.F7,
        KEY_F8 = Keys.F8,
        KEY_F9 = Keys.F9,
        KEY_F10 = Keys.F10,
        KEY_F11 = Keys.F11,
        KEY_F12 = Keys.F12,
        KEY_F13 = Keys.F13,
        KEY_F14 = Keys.F14,
        KEY_F15 = Keys.F15,
        KEY_F16 = Keys.F16,
        KEY_F17 = Keys.F17,
        KEY_F18 = Keys.F18,
        KEY_F19 = Keys.F19,
        KEY_F20 = Keys.F20,
        KEY_F21 = Keys.F21,
        KEY_F22 = Keys.F22,
        KEY_F23 = Keys.F23,
        KEY_F24 = Keys.F24,
        ALT = Keys.LMenu,
        SHIFT = Keys.Shift,
        //KEY_TICK = 
        KEY_SPACE = Keys.Space,
        KEY_DEL = Keys.Delete,
        KEY_PGUP = Keys.PageUp,
        KEY_PGDN = Keys.PageDown,
        KEY_HOME = Keys.Home,
        KEY_PERIOD = Keys.OemPeriod, // TODO: CHECK
        KEY_END = Keys.End,
        CTRL = Keys.ControlKey, // TODO: NEED REFINEMENT?
        KEY_INS = Keys.Insert,
        KEY_TAB = Keys.Tab,
        KEY_BACKSPACE = Keys.Back,
        KEY_KP5 = 42, // middle mouse
        KEY_KP4 = 42, // ??
        KEY_KP6 = 42, // ??
        KEY_DOWN = Keys.Down,
        KEY_UP= Keys.Up,
        KEY_LEFT = Keys.Left,
        KEY_RIGHT = Keys.Right,
        KEY_ENTER = Keys.Enter,
    };
    
    public class Ra3HotkeyData
    {

        // TODO:
        // * Mapping Keys <--> Ra3 Hotkey 
        // * Find a way to unique key usages (set of available vs set of used?)
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SellMode")]
        public List<Keys> SellMode { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("ToggleRepairMode")]
        public List<Keys> RepairMode { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("OpenPauseScreen")]
        public List<Keys> Pause { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarSelectionRefinementPage")]
        public List<Keys> ContextualTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarMainStructurePage")]
        public List<Keys> ProductionStructureTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarOtherStructurePage")]
        public List<Keys> SupportStructureTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarInfantryPage")]
        public List<Keys> InfantryTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarVehiclePage")]
        public List<Keys> VehicleTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarAircraftPage")]
        public List<Keys> AircraftTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarWatercraftPage")]
        public List<Keys> NavyTab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("GoToNextSubGroup")]
        public List<Keys> CycleUnitSubgroup { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("GoToPriorSubGroup")]
        public List<Keys> CyclePreviousUnitSubgroup { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("GoToNextBuildQueue")]
        public List<Keys> NextSubtab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("GoToPriorBuildQueue")]
        public List<Keys> PreviousSubtab { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot1")]
        public List<Keys> Slot1 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot2")]
        public List<Keys> Slot2 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot3")]
        public List<Keys> Slot3 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot4")]
        public List<Keys> Slot4 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot5")]
        public List<Keys> Slot5 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot6")]
        public List<Keys> Slot6 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot7")]
        public List<Keys> Slot7 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot8")]
        public List<Keys> Slot8 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("SideBarButtonSlot9")]
        public List<Keys> Slot9 { get; set; }
        [HotkeyCategory("Sidebar")]
        [HotkeyName("OpenAdvancedCommands")]
        public List<Keys> ShowAdvancedCommands { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("UnitAbilityButtonSlot1")]
        public List<Keys> UseUnitSpecialAbility { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("AttackMove")]
        public List<Keys> AttackMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("ReverseMove")]
        public List<Keys> ReverseMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("ForceMove")]
        public List<Keys> CrushMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("Stop")]
        public List<Keys> StopUnits { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("ForceAttack")]
        public List<Keys> ForceAttack { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("WaypointMode")]
        public List<Keys> WaypointMode { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("PlanningMode")]
        public List<Keys> PlanningMode { get; set; }
        [HotkeyCategory("UnitCommands", true)]
        [HotkeyName("CycleHarvesters")]
        public List<Keys> CycleHarvesters { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("StanceAggressive")]
        public List<Keys> AggressiveStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("StanceGuard")]
        public List<Keys> GuardStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("StanceHoldPosition")]
        public List<Keys> HoldGroundStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        [HotkeyName("StanceHoldFire")]
        public List<Keys> HoldFireStance { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("OpenPlayerTechStore")]
        public List<Keys> OpenSupportPowersMenu { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot1")]
        public List<Keys> SSlot1 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot2")]
        public List<Keys> SSlot2 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot3")]
        public List<Keys> SSlot3 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot4")]
        public List<Keys> SSlot4 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot5")]
        public List<Keys> SSlot5 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot6")]
        public List<Keys> SSlot6 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot7")]
        public List<Keys> SSlot7 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot8")]
        public List<Keys> SSlot8 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot9")]
        public List<Keys> SSlot9 { get; set; }
        [HotkeyCategory("SupportPowers")]
        [HotkeyName("PlayerPowerButtonSlot10")]
        public List<Keys> SSlot10 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraReset")]
        public List<Keys> ResetCamera { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraScrollLeft")]
        public List<Keys> ScrollViewToTheLeft { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraScrollRight")]
        public List<Keys> ScrollViewToTheRight { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraScrollUp")]
        public List<Keys> ScrollViewForward { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraScrollDown")]
        public List<Keys> ScrollViewToTheBack { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraRotateLeft")]
        public List<Keys> RotateViewToTheLeft { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraRotateRight")]
        public List<Keys> RotateViewToTheRight { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraZoomIn")]
        public List<Keys> ZoomIn { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("CameraZoomOut")]
        public List<Keys> ZoomOut { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark1")]
        public List<Keys> GoToBookmark1 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark2")]
        public List<Keys> GoToBookmark2 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark3")]
        public List<Keys> GoToBookmark3 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark4")]
        public List<Keys> GoToBookmark4 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark5")]
        public List<Keys> GoToBookmark5 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark6")]
        public List<Keys> GoToBookmark6 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark7")]
        public List<Keys> GoToBookmark7 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("GoToViewBookmark8")]
        public List<Keys> GoToBookmark8 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark1")]
        public List<Keys> SetBookmark1 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark2")]
        public List<Keys> SetBookmark2 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark3")]
        public List<Keys> SetBookmark3 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark4")]
        public List<Keys> SetBookmark4 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark5")]
        public List<Keys> SetBookmark5 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark6")]
        public List<Keys> SetBookmark6 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark7")]
        public List<Keys> SetBookmark7 { get; set; }
        [HotkeyCategory("CameraControls")]
        [HotkeyName("SaveViewBookmark8")]
        public List<Keys> SetBookmark8 { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("ChatWithEveryone")]
        public List<Keys> ChatWithEveryone { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("ChatWithAllies")]
        public List<Keys> ChatWithAllies { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("VoiceChat")]
        public List<Keys> VoiceChat { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("ToggleVoiceChatMode")]
        public List<Keys> ToggleVoiceChatMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("CommunicateWithAlliesMode")]
        public List<Keys> BeaconMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("QuickChatMenu")]
        public List<Keys> QuickChatCommands { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("ToggleFastForwardMode")]
        public List<Keys> FastForwardMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("TelestratorToggle")]
        public List<Keys> TurnTelestratorOnOrOff { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("TelestratorErase")]
        public List<Keys> EraseTelestratorDrawings { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("TelestratorNextLineWidth")]
        public List<Keys> SelectTelestratorLineWidth { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("TelestratorNextColor")]
        public List<Keys> SelectNextTelestratorColor { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        [HotkeyName("TelestratorPriorColor")]
        public List<Keys> SelectPriorTelestratorColor { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("SelectAll")]
        public List<Keys> SelectAllUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("SelectMatchingUnits")]
        public List<Keys> SelectMatchingUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("ShowAllHealthBars")] 
        public List<Keys> ShowAllHealthbars { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("PlaceRallyPoint")]
        public List<Keys> PlaceRallyPoint { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> JumpToNextSupportUnit { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("ViewLastEvaEvent")]
        public List<Keys> ViewLastEvent { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("Scatter")]
        public List<Keys> ScatterUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("ViewHomeBase")]
        public List<Keys> ViewHomeBase { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("PreferSelection")]
        public List<Keys> AddToSelection { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("ToggleHUD")]
        public List<Keys> ToggleHud { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("OpenSaveGameScreen")]
        public List<Keys> OpenSaveMenu { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("OpenLoadGameScreen")]
        public List<Keys> OpenLoadMenu { get; set; }
        [HotkeyCategory("Miscellaneous")]
        [HotkeyName("TakeScreenShot")]
        public List<Keys> Screenshot { get; set; }
        [HotkeyCategory("Meta", true)]
        [HotkeyName("Version")]
        public int Version { get; set; }
    }
}
