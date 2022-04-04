using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Text _statusText;
    [SerializeField]
    private Player _player;
    private int _hp;
    private int _red;
    private int _green;
    private int _yellow;
    
    void Start()
    {
        _player.DamageTaken += (d, h) =>
        {
            _hp = h;
            UpdateStatusText();
        };
        _player.CubePicked += (c) =>
        {
            switch (c)
            {
                case CubeColor.Red:
                    _red++;
                    break;
                case CubeColor.Green:
                    _green++;
                    break;
                case CubeColor.Yellow:
                    _yellow++;
                    break;
                default:
                    break;
            }
            UpdateStatusText();
        };
        _player.Death += () => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _hp = _player.Health;
        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        _statusText.text = $"HEALTH {_hp}/{_player.MaximumHealth}\nRED {_red}\nGREEN {_green}\nYELLOW {_yellow}\n";
    }
}
