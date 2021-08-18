using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class ExcuteLua : MonoBehaviour
{

   
    public TextAsset textAsset; //Lua�ļ�

    private LuaTable luaTable; //Lua�еı����

    private Action _luaAwake; // lua�е�Awake����
    private Action _luaStart; // lua�е�Start����
    private Action _luaUpdate; // lua�е�Update����

    public List<UnityObj> unityObjs = new List<UnityObj>(); //��Ҫӳ�䵽Lua�е�Unity����


    private void Awake()
    {
        // Lua������ﴴ��һ����ͨ��
        luaTable = LuaManager.GetLuaEnv().NewTable();
        
        // Lua������ﴴ��һ��Ԫ��
        LuaTable metaTable = LuaManager.GetLuaEnv().NewTable();
        
        //��Ԫ������Ԫ����������ͨ����Ҳ�����Ӧ������ʱ����Ԫ���ڲ���
        metaTable.Set("__index", LuaManager.GetLuaEnv().Global);

        //Ϊ��ͨ������Ԫ��
        luaTable.SetMetaTable(metaTable);

        //�ͷ�Ԫ��
        metaTable.Dispose();

        //�����thisָ���luaTable��self������
        luaTable.Set("self", this);

        //��Unity���� lua�ڲ�������lua�޸�unity����
        foreach (UnityObj item in unityObjs)
        {
            luaTable.Set(item.name, item.obj);
        }


        //ִ��Lua�ļ�
        LuaManager.GetLuaEnv().DoString(textAsset.text, "LuaModule.lua", luaTable);

        //��C#������lua������
        _luaAwake = luaTable.Get<Action>("LuaAwake");
        _luaStart = luaTable.Get<Action>("LuaStart");
        _luaUpdate = luaTable.Get<Action>("LuaUpdate");


        if(_luaAwake != null)
        {
            //ִ��lua Awake����
            _luaAwake();
        }

    }

    private void Start()
    {
        if (_luaStart != null)
        {
            //ִ��lua Start����
            _luaStart();
        }

    }

    private void Update()
    {
        if(_luaUpdate != null)
        {
            //ִ��lua Update����
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

