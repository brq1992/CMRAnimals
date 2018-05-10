using UnityEngine;

 namespace AppUtility
{
    public static class CommonMethod
    {
        public static string[] GetThreePartContent(string content)
        {
            string[] strs = content.Split('@');
            if(strs.Length!=3)
            {
                Debug.LogError("string array's length is wrong! content :" + content);
            }
            return strs;
        }
    }
}
