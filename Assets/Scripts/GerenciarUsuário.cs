using AnotherFileBrowser.Windows;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GerenciarUsuário : MonoBehaviour
{
    public RawImage rawImage;
    private string imagePath;
    private string defaultImagePath;

    public TMP_Text usernameText;
    public TMP_InputField inputField; // Reference to your InputField

    void Awake()
    {
        defaultImagePath = Application.persistentDataPath + "/defaultImage.png";
    }

    public void AbrirExplorer()
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            StartCoroutine(LoadImage(path));
        });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

#pragma warning disable CS0618 // Type or member is obsolete
            if (uwr.isNetworkError || uwr.isHttpError)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                rawImage.texture = uwrTexture;

                byte[] bytes = uwrTexture.EncodeToPNG();
                string imageName = "savedImage.png";
                imagePath = Path.Combine(Application.persistentDataPath, imageName);
                File.WriteAllBytes(imagePath, bytes);

                PlayerPrefs.SetString("ImagePath", imagePath);
            }
        }
    }

    public void SalvarNome()
    {
        string username = inputField.text;
        PlayerPrefs.SetString("Username", username);
        if (usernameText != null)
        {
            usernameText.text = PlayerPrefs.GetString("Username");
        }
        PlayerPrefs.Save();
    }

    public void ResetarUsuario()
    {
        if (File.Exists(imagePath) && imagePath != defaultImagePath)
        {
            File.Delete(imagePath);
        }
        PlayerPrefs.DeleteKey("Username");
        usernameText.text = "";
        rawImage.texture = null;
    }

    // Carrega a imagem
    void Start()
    {
        string savedImagePath = PlayerPrefs.GetString("ImagePath", defaultImagePath);
        if (!string.IsNullOrEmpty(savedImagePath) && File.Exists(savedImagePath))
        {
            byte[] fileData = File.ReadAllBytes(savedImagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            rawImage.texture = texture;
        }

        if (usernameText != null)
        {
            usernameText.text = PlayerPrefs.GetString("Username");
        }
    }

    public void ResetarJogo()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
