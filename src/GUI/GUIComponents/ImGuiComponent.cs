using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using JsonAnything.Util;
using OpenTK;

namespace JsonAnything.GUI.GUIComponents
{
    public abstract class ImGuiComponent
    {
        protected readonly Dictionary<string, Modal> _modals;
        protected readonly MainWindow _window;
        protected readonly Queue<string> _modalsToCreate;
        protected readonly Queue<string> _modalsToDestroy;

        protected static int ModalCount = 0;

        protected ImGuiComponent(MainWindow window)
        {
            _modals = new Dictionary<string, Modal>();
            _modalsToCreate = new Queue<string>();
            _modalsToDestroy = new Queue<string>();
            _window = window;
        }
        
        public abstract void Render();

        protected void renderModals()
        {
            while (_modalsToCreate.Count > 0)
            {
                ImGui.OpenPopup(_modalsToCreate.Dequeue());
            }

            while (_modalsToDestroy.Count > 0)
            {
                _modals.Remove(_modalsToDestroy.Dequeue());
            }
            
            foreach (KeyValuePair<string, Modal> kv in _modals)
            {
                if (ImGui.BeginPopupModal(kv.Key, ref kv.Value.Active))
                {
                    kv.Value.Component.Render();
                    ImGui.EndPopup();
                }

                if (!kv.Value.Active)
                {
                    _modalsToDestroy.Enqueue(kv.Key);
                }
            }
        }

        protected void createModal(string name, ImGuiComponent component)
        {
            string actualName = $"{name}##{ModalCount++}";
            
            _modals[actualName] = new Modal(component);
            _modalsToCreate.Enqueue(actualName);
        }

        protected class Modal
        {
            public bool Active = true;
            public readonly ImGuiComponent Component;

            public Modal(ImGuiComponent component)
            {
                Component = component;
            }
        }
    }
}
