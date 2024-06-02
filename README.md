
# Unity System Patterns

💻 Unity System Patterns is a collection of scripts containing the most commonly used patterns for game development. Below is a description of each pattern, dependencies, and examples of use.

- [PubSub Pattern](#pubsub-pattern)
- [Object Pooling Pattern](#object-pooling-pattern)
- [Command Queue Pattern](#command-queue-pattern)
- [UI Controller with State Machine Pattern](#ui-controller-with-state-machine-pattern)
- [Singleton](#singleton)
- [MasterSound](#mastersound)


## PubSub Pattern

The PubSub (Publish-Subscribe) pattern for Unity in C# provides an efficient means of communication between objects, allowing them to communicate asynchronously and decoupled through messages published on specific channels. This pattern is particularly suitable for games with complex interactions between various game elements, such event-driven games with dynamic environments, where different game elements react to events triggered by others.

### Example Of Usage

#### Events
```c#
public class MyClassEvent
{
    public class MyClassScoredEvent
    {
        public int Score { get; private set; }

        public MyClassScoredEvent(int score)
        {
            Score = score;
        }
    }

    // Other events
}
```

#### Publish
```c#
using Nato.PubSub;
using static MyClassEvent;
public class MyClassLogic : MonoBehaviour
{
    private int score;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score++;
            MyClassScoredEvent playerScoredEvent = new MyClassScoredEvent(score);
            EventManager<MyClassScoredEvent>.Publish(playerScoredEvent);
        }
    }
}
```

#### Subscribe and Unsubscribe
```c#
using Nato.PubSub;
public class MyClassVisual : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager<MyClassScoredEvent>.Subscribe(OnPlayerScored);
    }

    private void OnDisable()
    {
        EventManager<MyClassScoredEvent>.Unsubscribe(OnPlayerScored);
    }

    private void OnPlayerScored(MyClassEvent.MyClassScoredEvent @event)
    {
        Debug.Log(@event.Score);
    }
}
```
## Object Pooling Pattern

Object pooling for Unity in C# provides an efficient way to manage and reuse game objects, reducing the overhead of object creation and destruction during runtime. This pattern is especially beneficial for games with a high frequency of instantiating and destroying similar objects, such as bullet-hell shooters, endless runners, or particle-heavy effects games like tower defense or strategy games with numerous units.

### Example Of Usage

#### Pool Manager
```c#
using Nato.Pool;
public class MyPoolManager
{
    [SerializeField] private Enemy enemyPrefab;

    private void Start(){
        ObjectPool<Enemy>.Create(enemyPrefab, size:10); // Can grow above the limit
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            SpawnEnemy();
        }
    }

    private void SpawnEnemy(){
        Enemy enemy = ObjectPool<Enemy>.Get();
        enemy.transform.position = Random.insideUnitSphere * 5f;
    }
}
```
#### Enemy
```c#
using Nato.Pool;
public class Enemy : MonoBehaviour, IPoolObject
{
    public void OnGettingFromPool() {}

    public void OnReturnToPool() {}

    private void OnMouseDown()
    {
        ObjectPool<Enemy>.Return(this);
    }
}
```
## Command Queue Pattern

The command queue for Unity in C# provides a streamlined method for managing and executing commands asynchronously, enabling effective control flow and decoupling between components through a queue-based system. This pattern is particularly suitable for strategy games, where complex sequences of player actions need to be processed

### Example Of Usage
#### ExampleCommand
```c#
using Nato.Command;
public class ExampleCommand : ICommand
{
    int value = 0;
    public ExampleCommand(int value)
    {
        this.value = value;
    }

    public IEnumerator Execute()
    {
        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);

        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);

        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);
    }
}
```

#### CommandManager
```c#
using Nato.Command;
[RequireComponent(typeof(CommandQueue))]
public class CommandManagerExample : MonoBehaviour
{
    private bool isCommandQueueExecuting = false;
    CommandQueue commandQueue;

    private void OnEnable()
    {
        commandQueue = gameObject.GetComponent<CommandQueue>();
        commandQueue.OnCommandsExecuted += CommandQueue_OnCommandsExecuted;
    }

    private void OnDestroy()
    {
        commandQueue.OnCommandsExecuted -= CommandQueue_OnCommandsExecuted;
    }

    private void CommandQueue_OnCommandsExecuted()
    {
        Debug.Log("All Commands Executed");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ExecuteCommandsAsync(new ExampleCommand(value: 0), () =>
            {
                Debug.Log("End of commands");
            });
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            EnqueueAndExecuteCommands(new ExampleCommand(value: 0), () =>
            {
                Debug.Log("End of commands");
            });
        }
    }

    public void ExecuteCommandsAsync(ICommand command, Action callback=null)
    {
        commandQueue.EnqueueCommand(command);
        commandQueue.ExecuteCommands(callback);
    }

    public void EnqueueAndExecuteCommands(ICommand command, Action callback=null)
    {
        commandQueue.EnqueueCommand(command);
        if (!isCommandQueueExecuting)
        {
            if (commandQueue.commandQueue.Count > 0)
            {
                isCommandQueueExecuting = true;
                commandQueue.ExecuteCommands(() =>
                {

                    isCommandQueueExecuting = false;
                    callback?.Invoke();
                });
            }
        }
    }
}
```
## UI Controller with State Machine Pattern

The StateMachine pattern for Unity in C# provides a structured approach for managing the behavior of game entities by defining a set of states and transitions between them. This enables efficient control over the entity's actions and interactions within the game world. State machines are well-suited for games with dynamic and complex character behaviors or UI states.

### Example Of Usage
#### UIManager
```c#
public class UIManager : MonoBehaviour
{
    [field: SerializeField] public GameplayUI GameplayUI { get; private set; }
    [field: SerializeField] public MenuUI MenuUI { get; private set; }


    public void DisableAll()
    {
        GameplayUI?.Disable();
        MenuUI?.Disable();
    }
}
```
#### Example UI State Manager
```c#
public class ExampleStateManager : MonoBehaviour
{
    public static ExampleStateManager Instance { get; private set; }

    public UIManager UIManager;
    public StateMachine<ExampleStateManager> StateMachine { get; private set; }

    private void Awake()
    {
        Instance = this;
        StateMachine = new(this);
    }

    private void Start()
    {
        StateMachine.TransitionTo<MenuState>();
    }

    private void Update()
    {
        StateMachine.OnTick();
    }
}
```

#### GameplayState (Logic)
```c#
public class GameplayState : BaseState<ExampleStateManager>
{
    private float timer = 0;

    public override void OnStart(ExampleStateManager gameManager)
    {
        base.OnStart(gameManager);

        Manager.UIManager.GameplayUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();

        Manager.UIManager.GameplayUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();

        timer += Time.deltaTime;
        Manager.UIManager.GameplayUI.SetTimer(timer);


        if (Input.GetKeyDown(KeyCode.Space))
            Manager.StateMachine.TransitionTo<MenuState>();
    }

    public void SetInitialTimer(float timer)
    {
        this.timer = timer; 
    }
}
```

#### GameplayUI (Visual)
```c#
public class GameplayUI : BaseUI
{
    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
    }

    public void SetTimer(float time)
    {
        int timeMs = Mathf.CeilToInt(time) * 1000;
        TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, timeMs);
        Debug.Log($"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}");
    }
}
```

## Singleton
The Singleton pattern in Unity, implemented in C#, ensures that a class has only one instance and provides a global point of access to it. This pattern is particularly useful in game development for managing game states, settings, or any other class where a single instance is sufficient or required. The Singleton pattern helps in maintaining consistent states and data across different scenes and components, providing a controlled and centralized way of accessing shared resources.

### Example Of Usage
```c#

// GameManager uses the basic Singleton pattern. It ensures only one instance of GameManager exists,
// and destroys any new instance created. This is useful for managing the game's state, score, etc.
public class GameManager : Singleton<GameManager>
{
    public int Score { get; private set; }

    public void AddScore(int value)
    {
        Score += value;
    }
}

// SceneTransition uses the Persistent Singleton pattern. It ensures only one instance of SceneTransition exists and persists across scene loads
public class SceneTransition : PersistentSingleton<SceneTransition>
{
    public void LoadScene(string sceneName){}
}


// AudioManager uses the Singleton Auto Instance pattern. It automatically creates an instance if it doesn't exist
// when accessed, making it suitable for on-demand instance creation and avoiding the need to manually create the instance.
public class AudioManager : SingletonAutoInstance<AudioManager>
{
    public void PlayAudio(AudioClip audioClip){}
}

// Localization uses the Persistent Singleton Auto Instance pattern. It ensures only one instance of Localization exists,
// automatically creates it if needed, and persists it across scene loads, making it suitable for managing globally.
public class Localization : PersistentSingletonAutoInstance<Localization>
{
    public void ChangeLanguage(Language language){}
}

```

## MasterSound
MasterSound is an extension for controlling an audio framework called [AudioPhile](https://github.com/pixel-dust-dev/Unity-Audiophile.git). To function correctly, the following dependencies need to be added:

[Guide to Install Packages via Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
- https://github.com/pixel-dust-dev/Unity-Audiophile.git
- https://github.com/pixel-dust-dev/Unity-EditorUtilities.git
- https://github.com/pixel-dust-dev/Unity-WeightedObjects.git

MasterSound allows you to harness the power of streamlined Sound Effect implementation with the efficiency of Unity's AudioMixer, adding the ability to handle Background Music with a CrossFade option and providing the option to save volume settings between the AudioMixer groups.

### Example Of Usage
#### MyClassBehaviour
```c#
using Nato.Sound;

[SerializeField] private SoundEvent bgmSound1;
[SerializeField] private SoundEvent bgmSound2;
[SerializeField] private SoundEvent sfxSound;

// Play sounds
MasterSound.PlayMusic(bgmSound1); // Background music 
MasterSound.CrossfadeTo(bgmSound2, durationFade: 1, delay: 0); // Background music crossfade
MasterSound.Play(sfxSound); // SFX
MasterSound.PlayAt(sfxSound, transform); // SFX with spatial localization

// Saves settings
MasterSound.SaveVolume(volume: 0, MasterSound.AUDIO_OUTPUT.MUSIC); // Save BGM volume
MasterSound.SaveVolume(volume: 0, MasterSound.AUDIO_OUTPUT.EFFECT); // Save SFX volume
MasterSound.GetSavedVolume(MasterSound.AUDIO_OUTPUT.MUSIC); // Load BGM volume
MasterSound.GetSavedVolume(MasterSound.AUDIO_OUTPUT.EFFECT); // Load SFX volume
```
## 🔗 Links
- Author: [@natomarcacini](https://www.github.com/renatomarcacini)

  
[![portfolio](https://img.shields.io/badge/my_portfolio-000?style=for-the-badge&logo=ko-fi&logoColor=white)](https://renatomarcacini.github.io/portfolio/)
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/renato-gomes-marcacini-50b78b1a7)


