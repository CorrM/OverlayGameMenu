using System.Collections.Generic;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace OverlayGameMenu
{
    public static class KeyBordManager
    {
        private static InputSimulator input = new InputSimulator();
        private static bool IsKeyThreadRunning = false;

        private static bool IsKeyPressed(this IInputDeviceStateAdaptor adaptor, VirtualKeyCode keyCode)
        {
            if(adaptor.IsKeyDown(keyCode))
            {
                while (!input.InputDeviceState.IsKeyUp(keyCode))
                    Thread.Sleep(1);

                return true;
            }
            return false;
        }

        public static void KeyDetector(MenuBase menu, List<VirtualKeyCode> keyCodesToHandle)
        {
            if (!IsKeyThreadRunning)
            {
                IsKeyThreadRunning = true;
                new Thread(() =>
                {
                    while (true)
                    {
                        for (int i = 0; i < keyCodesToHandle.Count; i++)
                            if (input.InputDeviceState.IsKeyPressed(keyCodesToHandle[i]))
                                menu.MenuKeyPressed(keyCodesToHandle[i]);

                        Thread.Sleep(2);
                    }
                }).Start();
            }
        }
    }
}
