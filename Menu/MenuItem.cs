using System;

namespace OverlayGameMenu
{
    public enum OptionType
    {
        Expanded,
        Custom
    }

    public class MenuItem
    {
        public int index;

        public MenuItemID ItemID { get; set; }
        public Type EnumType { get; set; }
        public MenuItem Parent { get; set; }

        public bool IsSubOpt { get; set; }
        public int ChieldCount { get; set; }

        public string OptName { get; set; }
        public OptionType OptType { get; set; }

        public int OptState {
            get { return ItemID.ChoseID; }
            set { ItemID.ChoseID = value; ItemID.Activated = value != (int)MenuItemTypes.OnOffItem.Off; }
        }

        /// <summary>
        /// To inti normal and sub option
        /// </summary>
        public MenuItem(MenuItemID OptionID, string OptionName, Type EnumType, bool Expanded = false)
        {
            Inti(OptionID, OptionName, EnumType, Expanded);
        }

        private void Inti(MenuItemID OptionID, string OptionName, Type EnumType, bool Expanded)
        {
            if (!EnumType.IsEnum)
                throw new ArgumentException("EnumType must be an enumerated type");

            this.EnumType = EnumType;
            this.OptName = OptionName;
            this.ItemID = OptionID;
            this.OptType = Expanded ? OptionType.Expanded : OptionType.Custom;
            this.OptState = (int)Enum.GetValues(EnumType).GetValue(0);
        }

        public void NextValue()
        {
            if (OptState < Enum.GetValues(EnumType).Length - 1)
                OptState++;
        }
        public void PrevValue()
        {
            if (OptState > 0)
                OptState--;
        }
        public string GetValueName()
        {
            return System.Text.RegularExpressions.Regex.Replace(Enum.GetName(EnumType, OptState), "(\\B[A-Z])", " $1");
        }
    }
}
