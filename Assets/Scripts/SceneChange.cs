// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SceneChange : MonoBehaviour
// {
//     public string menuScene;
//     public string gameScene;

//     public void Play()
//     {
//         SceneManager.LoadScene(gameScene);
//     }

//     public void BackToMenu()
//     {
//         SceneManager.LoadScene(menuScene);
//     }

//     public void Reset()
//     {
//         // Reset điểm TRƯỚC khi load lại scene, vì lúc này GameManager.Instance
//         // chắc chắn còn tồn tại (miễn là GameManager đã có mặt ngay từ MenuScene).
//         if (GameManager.Instance != null)
//         {
//             GameManager.Instance.ResetScore();
//         }
//         else
//         {
//             Debug.LogWarning("[SceneChange] GameManager.Instance đang null - " +
//                 "kiểm tra xem GameManager đã được đặt trong scene khởi động (MenuScene) chưa.");
//         }

//         SceneManager.LoadScene(gameScene);
//     }
// }