using System;

using UnityEngine;

#if UNITY_ANDROID
using System.Threading.Tasks;

using UnityEngine.Android;
#endif
public class AndroidPermissionRequester: MonoBehaviour 
{
        
  

#if UNITY_ANDROID
    async void Start()
    {
      await RequestPermissionsAsync();
    }

    private async Task RequestPermissionsAsync()
    {
      foreach (var permission in _permissions)
      {
        if (!PermissionRequester.HasPermission(permission))
        {
          await PermissionRequester.RequestPermissionAsync(permission);
        }
      }
    }
#endif
    }

