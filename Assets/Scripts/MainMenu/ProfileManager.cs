using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{

    public static ProfileManager ProfileM;


    public GameObject vehiclePreview;
    public GameObject vehiclePreviewGallery;

    private ObjectID actualVehicle;
    private List<int> vehiclesIDs = new List<int>();
    int actualIndex, countVehicle;


    private void Awake()
    {
        ProfileM = this;
    }

    public void PrepareProfile()
    {
        vehiclesIDs.Clear();
        foreach (string value in UserInfo.UserI.GetVehicles().Keys)
        {
            vehiclesIDs.Add(int.Parse(value));
        }
        if (UserInfo.UserI.bunny.ContainsKey("box"))
        {
            ShowVehicle(int.Parse(UserInfo.UserI.bunny["box"].ToString()));
        }
        else {
            ShowVehicle(1);
        }
    }

    void ShowVehicle(int ID) {
        for (int x = 0; x < vehiclePreview.transform.childCount; x++)
            Destroy(vehiclePreview.transform.GetChild(x).gameObject);
        GameObject vehicle = ObjectsManager.ObjectsM.InstantiateVehicleInGame(ID, vehiclePreview);
        vehiclePreview.GetComponent<Animation>().Play("vehicle_preview");
        vehiclePreviewGallery.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ObjectsManager.ObjectsM.GetNameVehicle(ID);

        countVehicle = vehiclesIDs.Count;
        actualIndex = vehiclesIDs.IndexOf(ID);

        vehiclePreviewGallery.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", actualIndex+1, countVehicle);
        if (actualIndex + 1 >= countVehicle)
        {
            vehiclePreviewGallery.transform.GetChild(4).GetComponent<Button>().interactable = false;
        }
        else {
            vehiclePreviewGallery.transform.GetChild(4).GetComponent<Button>().interactable = true;
        }

        if (actualIndex == 0)
        {
            vehiclePreviewGallery.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }
        else
        {
            vehiclePreviewGallery.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }
        UserInfo.UserI.bunny["box"] = ID;
    }

    public void ChangeVehicle(int opc) {
        if (opc > 0)
        {
            if(actualIndex + 2 <= countVehicle)
                ShowVehicle(vehiclesIDs[actualIndex + 1]);
        }
        else
        {
            if (actualIndex - 1 >= 0)
                ShowVehicle(vehiclesIDs[actualIndex - 1]);
        }
    }

}
