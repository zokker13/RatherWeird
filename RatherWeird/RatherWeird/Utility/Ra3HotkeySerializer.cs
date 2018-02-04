using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RatherWeird.Utility
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HotkeyNameAttribute : Attribute
    {
        public HotkeyNameAttribute(string la)
        {
            Lalala = la;
        }

        public string Lalala;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HotkeyCategory : Attribute
    {
        public HotkeyCategory(string category)
        {
            Category = category;
        }

        public string Category;
    }

    public class Ra3HotkeySerializer
    {
        public static async Task<Ra3HotkeyData> ReadHotkeyFile(string path)
        {
            Ra3HotkeyData data = new Ra3HotkeyData();

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

                        Console.WriteLine($"-> {key}: {value}");
                    }
                }
                
                
            }

            return data;
        }
    }
    
    public class Ra3HotkeyData
    {

        // TODO:
        // * Mapping Keys <--> Ra3 Hotkey 
        // * Find a way to unique key usages (set of available vs set of used?)
        [HotkeyCategory("Sidebar")]
        public List<Keys> SellMode { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> RepairMode { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Pause { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> ContextualTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> ProductionStructureTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> SupportStructureTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> InfantryTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> VehicleTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> AircraftTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> NavyTab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> CycleUnitSubgroup { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> CyclePreviousUnitSubgroup { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> NextSubtab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> PreviousSubtab { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot1 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot2 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot3 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot4 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot5 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot6 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot7 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot8 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> Slot9 { get; set; }
        [HotkeyCategory("Sidebar")]
        public List<Keys> ShowAdvancedCommands { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> UseUnitSpecialAbility { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> AttackMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> ReverseMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> CrushMove { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> StopUnits { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> ForceAttack { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> WaypointMode { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> PlanningMode { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> AggressiveStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> GuardStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> HoldGroundStance { get; set; }
        [HotkeyCategory("UnitCommands")]
        public List<Keys> HoldFireStance { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> OpenSupportPowersMenu { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot1 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot2 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot3 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot4 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot5 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot6 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot7 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot8 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> SSlot9 { get; set; }
        [HotkeyCategory("SupportPowers")]
        public List<Keys> Slot10 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ResetCamera { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ScrollViewToTheLeft { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ScrollViewToTheRight { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ScrollViewForward { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ScrollViewToTheBack { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> RotateViewToTheLeft { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> RotateViewToTheRight { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ZoomIn { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> ZoomOut { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark1 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark2 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark3 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark4 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark5 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark6 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark7 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> GoToBookmark8 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark1 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark2 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark3 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark4 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark5 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark6 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark7 { get; set; }
        [HotkeyCategory("CameraControls")]
        public List<Keys> SetBookmark8 { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> ChatWithEveryone { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> ChatWithAllies { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> VoiceChat { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> ToggleVoiceChatMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> BeaconMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> QuickChatCommands { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> FastForwardMode { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> TurnTelestratorOnOrOff { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> EraseTelestratorDrawings { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> SelectTelestratorLineWidth { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> SelectNextTelestratorColor { get; set; }
        [HotkeyCategory("MultiplayerChat")]
        public List<Keys> SelectPriorTelestratorColor { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> SelectAllUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> SelectMatchingUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> ShowAllHealthbars { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> PlaceRallyPoint { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> JumpToNextSupportUnit { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> ViewLastEvent { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> ScatterUnits { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> ViewHomeBase { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> AddToSelection { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> ToggleHud { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> OpenSaveMenu { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> OpenLoadMenu { get; set; }
        [HotkeyCategory("Miscellaneous")]
        public List<Keys> Screenshot { get; set; }
    }
}
