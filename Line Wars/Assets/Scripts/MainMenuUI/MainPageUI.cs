using System.Diagnostics;
using TMPro;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace MainMenuUI
{
    public class MainPageUI : MenuPageUI
    {
        private const string DefaultAddress = "127.0.0.1";
        private const string DefaultPort = "7777";
        
        [SerializeField] private TMP_InputField addressInputField;
        [SerializeField] private TMP_InputField portInputField;

        [Inject] private IHostManager _hostManager;

        public bool UseDefaultAddress { get; set; }
        
        private void Start()
        {
            addressInputField.text = DefaultAddress;
            portInputField.text = DefaultPort;
            ToggleUseDefaultAddress(false);
        }

        public void StartLobby()
        {
            _hostManager.HostGame();
        }

        public void StartGame()
        {
            _hostManager.StartGameCountdown();
        }

        public void StartClient()
        {
            var address = UseDefaultAddress ? _hostManager.DefaultAddress : addressInputField.text;
            
            if(UseDefaultAddress)
                Debug.Log($"Using default address for client");
            
            _hostManager.StartClient(address, ushort.Parse(portInputField.text));
        }
        
        public void ToggleUseDefaultAddress(bool value)
        {
            UseDefaultAddress = value;
            addressInputField.interactable = !value;
            portInputField.interactable = !value;

            if (!value)
            {
                portInputField.text = DefaultPort;
            }
        }

        public void StartLocalServer()
        {
            var pathToServerExe = Application.dataPath + "/WinServer/Feed The Beast.exe";
            Debug.Log(pathToServerExe);
            Process process = new Process();
            process.StartInfo.FileName = pathToServerExe;
        }
    }
}