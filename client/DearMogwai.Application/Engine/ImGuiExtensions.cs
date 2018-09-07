using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DearMogwai.Application.Engine
{
    public static class ImGuiExtensions
    {
        public static void Table<T>(this IEnumerable<T> items, string id, ref T selectedItem)
        {
            var props = typeof(T).GetProperties();

            ImGui.BeginChild("##scrolling");

            ImGui.Columns(props.Length, "##columns", true);

            PropertyInfo idProp = null;

            foreach (var prop in props)
            {
                if (prop.Name == id)
                    idProp = prop;
                ImGui.Text(prop.Name);
                ImGui.NextColumn();
            }

            float itemHeight = ImGuiNative.igGetTextLineHeightWithSpacing();
            int displayStart = 0, displayEnd = items.Count();
            ImGui.CalcListClipping(displayEnd, itemHeight, ref displayStart, ref displayEnd);
            var visibleitems = items.Skip(displayStart).Take(displayEnd - displayStart).ToArray();

            foreach (var prop in props)
            {
                ImGuiNative.igSetCursorPosY(ImGuiNative.igGetCursorPosY() + (displayStart * itemHeight));

                foreach (var i in visibleitems)
                {
                    if (ImGui.Selectable(prop.GetValue(i).ToString() + "##" + idProp?.GetValue(i).ToString(),
                        i.Equals(selectedItem)))
                    {
                        selectedItem = i;
                    }
                }
                ImGui.NextColumn();
            }

            ImGui.Columns(1, "##columns", true);

            ImGui.EndChild();
        }
    }
}
