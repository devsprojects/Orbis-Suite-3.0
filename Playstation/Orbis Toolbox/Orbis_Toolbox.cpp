#include "Common.h"
#include "Settings_Menu.h"
#include "System_Monitor.h"
#include "GamePad.h"

extern "C"
{
	int __cdecl module_start1(size_t argc, const void *args)
	{
		klog("!! Hello World !!\n");

		Mono::Init();

		if (GamePad::IsDown(GamePad::Buttons::Left | GamePad::Buttons::Triangle))
		{
			Notify("Orbis Toolbox: Aborting Launch!!");
			return 0;
		}

		System_Monitor::Init();
		Settings_Menu::Init();
		//Title_Menu::Init();	

		Notify(ORBIS_TOOLBOX_NOTIFY);
		
		return 0;
	}

	int __cdecl module_stop1(size_t argc, const void *args)
	{
		klog("!! BYE !!\n");

		Settings_Menu::Term();
		System_Monitor::Term();
		//Title_Menu::Term();

		sceKernelSleep(4);

		return 0;
	}

	void _start()
	{

	}
}