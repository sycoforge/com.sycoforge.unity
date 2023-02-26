using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ch.sycoforge.Unity.Util
{

    public static class Caller
    {
        public static object GetStaticField(string typeName, string fieldName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            Type type = Type.GetType(typeName);
            var field = type.GetField(fieldName, flags);

            return field.GetValue(null);
        }

        public static object GetStaticField(Type type, string fieldName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            var field = type.GetField(fieldName, flags);

            return field.GetValue(null);
        }

        public static T GetStaticField<T>(Type type, string fieldName, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static)
        {
            var field = type.GetField(fieldName, flags);

            return (T)field.GetValue(null);
        }

        public static object GetField(object obj, string fieldName, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | flags);

            return field.GetValue(obj);
        }

        public static T GetField<T>(object obj, string fieldName, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | flags);

            if (field != null)
            {
                return (T)field.GetValue(obj);
            }
            else
            {
                return default(T);
            }
        }

        public static void SetField(object obj, string fieldName, object value, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = obj.GetType();

            var field = type.GetField(fieldName, BindingFlags.Instance | flags);

            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }

        public static void SetStaticField(Type type, string fieldName, object value, BindingFlags flags = BindingFlags.NonPublic)
        {
            var field = type.GetField(fieldName, BindingFlags.Static | flags);

            field.SetValue(null, value);
        }

        public static void SetProperty(object obj, string propertyName, object value, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = obj.GetType();

            var property = type.GetProperty(propertyName, flags);

            property.SetValue(obj, value, null);
        }

        public static void SetStaticProperty(string typeName, string propertyName, object value, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = Type.GetType(typeName);
            var property = type.GetProperty(propertyName, BindingFlags.Static | flags);

            property.SetValue(null, value, null);
        }

        public static void SetStaticProperty(Type type, string propertyName, object value, BindingFlags flags = BindingFlags.NonPublic)
        {
            var property = type.GetProperty(propertyName, BindingFlags.Static | flags);

            property.SetValue(null, value, null);
        }

        public static object GetStaticProperty(string typeName, string propertyName, BindingFlags flags = BindingFlags.NonPublic)
        {
            Type type = Type.GetType(typeName);
            var property = type.GetProperty(propertyName, BindingFlags.Static | flags);

            return property.GetValue(null, null);
        }

        public static object GetStaticProperty(Type type, string propertyName, BindingFlags flags = BindingFlags.NonPublic)
        {
            var property = type.GetProperty(propertyName, BindingFlags.Static | flags);

            return property.GetValue(null, null);
        }

        public static T GetProperty<T>(object obj, string propertyName)
        {
            Type type = obj.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.NonPublic);

            return (T)property.GetValue(obj, null);
        }


        public static object Call(object obj, string methodName, BindingFlags flags, Type[] types, params object[] args)
        {

            Type type = obj.GetType();


            MethodInfo method = type.GetMethod(methodName, types);

            return method.Invoke(obj, args);
        }

        public static object Call(object obj, string methodName, BindingFlags flags, params object[] args)
        {
            Type type = obj.GetType();


            Type[] types = new Type[args.Length];

            MethodInfo method = null;// type.GetMethod(methodName);


            try
            {
                method = type.GetMethod(methodName, flags);
            }
            catch (Exception)
            {

                if (args != null)
                {
                    types = new Type[args.Length];

                    for (int i = 0; i < args.Length; i++)
                    {

                        if (args[i] != null)
                        {
                            types[i] = args[i].GetType();
                        }
                        else
                        {
                            types[i] = typeof(object);
                        }

                    }

                    method = type.GetMethod(methodName, flags, null, types, null);
                }
                else
                {
                    method = type.GetMethod(methodName, flags);
                    //var method = type.GetMethod(methodName, types);
                }
            }


            return method.Invoke(obj, args);
        }

        public static T Call<T>(object obj, string methodName, BindingFlags flags, params object[] args)
        {
            Type type = obj.GetType();
            var method = type.GetMethod(methodName, flags);

            return (T)method.Invoke(obj, args);
        }

        public static object StaticCall(Type type, string methodName, BindingFlags flags, params object[] args)
        {
            //var method = type.GetMethod(methodName, BindingFlags.Static | flags);
            MethodInfo method = null;

            if (args != null && args.Length > 0)
            {
                Type[] types = new Type[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    types[i] = args[i].GetType();
                }

                method = type.GetMethod(methodName, BindingFlags.Static | flags, null, types, null);
            }
            else
            {
                method = type.GetMethod(methodName, BindingFlags.Static | flags);
            }

            return method.Invoke(null, args);
        }

        public static object StaticCall(string typeName, string methodName, params object[] args)
        {
            Type type = Type.GetType(typeName);
            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

            return method.Invoke(null, args);
        }

        public static T StaticCall<T>(string typeName, string methodName, params object[] args)
        {
            Type type = Type.GetType(typeName);
            var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

            return (T)method.Invoke(null, args);
        }

        public static T StaticCall<T>(Type type, string methodName, BindingFlags flags, params object[] args)
        {
            MethodInfo method;
            Type[] types = null;

            if (args != null && args.Length > 0)
            {
                types = new Type[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    types[i] = args[i].GetType();
                }

                method = type.GetMethod(methodName, BindingFlags.Static | flags, null, types, null);
            }
            else
            {
                method = type.GetMethod(methodName, BindingFlags.Static | flags);
                //var method = type.GetMethod(methodName, types);
            }

            return (T)method.Invoke(null, args);
        }


    }
}
