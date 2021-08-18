using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class ExcuteLua : MonoBehaviour
{

   
    public TextAsset textAsset; //Lua文件

    private LuaTable luaTable; //Lua中的表对象

    private Action _luaAwake; // lua中的Awake方法
    private Action _luaStart; // lua中的Start方法
    private Action _luaUpdate; // lua中的Update方法

    public List<UnityObj> unityObjs = new List<UnityObj>(); //需要映射到Lua中的Unity对象


    private void Awake()
    {
        // Lua虚拟机里创建一个普通表
        luaTable = LuaManager.GetLuaEnv().NewTable();
        
        // Lua虚拟机里创建一个元表
        LuaTable metaTable = LuaManager.GetLuaEnv().NewTable();
        
        //给元表设置元方法，当普通表查找不到对应的属性时，到元表内查找
        metaTable.Set("__index", LuaManager.GetLuaEnv().Global);

        //为普通表设置元表
        luaTable.SetMetaTable(metaTable);

        //释放元表
        metaTable.Dispose();

        //将类的this指针绑定luaTable的self变量上
        luaTable.Set("self", this);

        //绑定Unity对象到 lua内部，用于lua修改unity对象
        foreach (UnityObj item in unityObjs)
        {
            luaTable.Set(item.name, item.obj);
        }


        //执行Lua文件
        LuaManager.GetLuaEnv().DoString(textAsset.text, "LuaModule.lua", luaTable);

        //绑定C#依赖到lua函数上
        _luaAwake = luaTable.Get<Action>("LuaAwake");
        _luaStart = luaTable.Get<Action>("LuaStart");
        _luaUpdate = luaTable.Get<Action>("LuaUpdate");


        if(_luaAwake != null)
        {
            //执行lua Awake函数
            _luaAwake();
        }

    }

    private void Start()
    {
        if (_luaStart != null)
        {
            //执行lua Start函数
            _luaStart();
        }

    }

    private void Update()
    {
        if(_luaUpdate != null)
        {
            //执行lua Update函数
            _luaUpdate();
        }
        
       // Instantiate(Resources.Load("rect2"),)
            
       
    }


   
}

[Serializable]
public class UnityObj
{
    public string name;
    public GameObject obj;
}

