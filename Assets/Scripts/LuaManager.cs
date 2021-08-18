using XLua;

public class LuaManager
{
    private static LuaEnv luaEnv;
    public static LuaEnv GetLuaEnv()
    {
        if (luaEnv == null)
            luaEnv = new LuaEnv();
        return luaEnv;
    }
}
