using System;

namespace DearMogwai.Application.Engine.UiObjects
{
    public class MenuEntry : IEquatable<MenuEntry>
    {
        public MenuEntry Parent { get; set; }
        public string Identifier { get; }
        public string Label { get; set; }
        public Action Action { get; set; }
        public Func<bool> IsSelected { get; }
        public Func<bool> IsEnabled { get; }
        
        public string ComputedLabel => $"{Label}##{Identifier}";

        public MenuEntry(string label, string identifier, MenuEntry parent = null, Action action = null, Func<bool> isSelected = null, Func<bool> isEnabled = null)
        {
            Assert.IsNullOrWhitespace(label);
            Assert.IsNullOrWhitespace(identifier);

            Label = label;
            Identifier = identifier;
            Action = action;
            IsSelected = isSelected;
            IsEnabled = isEnabled;
            Parent = parent;
        }

        public bool Equals(MenuEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MenuEntry) obj);
        }

        public override int GetHashCode()
        {
            return (Identifier != null ? Identifier.GetHashCode() : 0);
        }
    }
}