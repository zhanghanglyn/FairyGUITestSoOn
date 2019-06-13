﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GGraphWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GGraph), typeof(FairyGUI.GObject));
		L.RegFunction("ReplaceMe", ReplaceMe);
		L.RegFunction("AddBeforeMe", AddBeforeMe);
		L.RegFunction("AddAfterMe", AddAfterMe);
		L.RegFunction("SetNativeObject", SetNativeObject);
		L.RegFunction("DrawRect", DrawRect);
		L.RegFunction("DrawRoundRect", DrawRoundRect);
		L.RegFunction("DrawEllipse", DrawEllipse);
		L.RegFunction("DrawPolygon", DrawPolygon);
		L.RegFunction("Setup_BeforeAdd", Setup_BeforeAdd);
		L.RegFunction("New", _CreateFairyGUI_GGraph);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("color", get_color, set_color);
		L.RegVar("shape", get_shape, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GGraph(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GGraph obj = new FairyGUI.GGraph();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GGraph.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReplaceMe(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.ReplaceMe(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddBeforeMe(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.AddBeforeMe(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddAfterMe(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.AddAfterMe(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNativeObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			FairyGUI.DisplayObject arg0 = (FairyGUI.DisplayObject)ToLua.CheckObject<FairyGUI.DisplayObject>(L, 2);
			obj.SetNativeObject(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawRect(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 6);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
			UnityEngine.Color arg3 = ToLua.ToColor(L, 5);
			UnityEngine.Color arg4 = ToLua.ToColor(L, 6);
			obj.DrawRect(arg0, arg1, arg2, arg3, arg4);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawRoundRect(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			UnityEngine.Color arg2 = ToLua.ToColor(L, 4);
			float[] arg3 = ToLua.CheckNumberArray<float>(L, 5);
			obj.DrawRoundRect(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawEllipse(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			UnityEngine.Color arg2 = ToLua.ToColor(L, 4);
			obj.DrawEllipse(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawPolygon(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			UnityEngine.Vector2[] arg2 = ToLua.CheckStructArray<UnityEngine.Vector2>(L, 4);
			UnityEngine.Color arg3 = ToLua.ToColor(L, 5);
			obj.DrawPolygon(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup_BeforeAdd(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)ToLua.CheckObject<FairyGUI.GGraph>(L, 1);
			FairyGUI.Utils.ByteBuffer arg0 = (FairyGUI.Utils.ByteBuffer)ToLua.CheckObject<FairyGUI.Utils.ByteBuffer>(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
			obj.Setup_BeforeAdd(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_color(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)o;
			UnityEngine.Color ret = obj.color;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index color on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_shape(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)o;
			FairyGUI.Shape ret = obj.shape;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index shape on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_color(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GGraph obj = (FairyGUI.GGraph)o;
			UnityEngine.Color arg0 = ToLua.ToColor(L, 2);
			obj.color = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index color on a nil value");
		}
	}
}
