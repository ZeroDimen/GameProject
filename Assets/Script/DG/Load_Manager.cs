using System;
using System.IO;
using TMPro;
using UnityEngine;

public class Load_Manager : MonoBehaviour
{
   public TMP_Text[] Text_Location;
   public TMP_Text[] Text_Time;
   
   public void Start()
   {
      for (int i = 0; i < 3; i++)
      {
         if (File.Exists(Data_Manager.instance.path+ $"{i}"))
         {
            PlayerData NowData = Data_Manager.instance.LoadData(i);
            if (NowData.Location != "No Data")
            {
               Text_Location[i].text = NowData.Location;
               Text_Time[i].text = NowData.Time;
            }
         }
         else
         {
            Text_Location[i].text = "No Data";
            Text_Time[i].text = "No Data";
         }
      }
   }
   
}
