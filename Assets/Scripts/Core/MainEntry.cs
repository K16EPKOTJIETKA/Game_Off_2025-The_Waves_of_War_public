using UnityEngine;
using Injection;
using TickMono;
using SequenceRunner;
using ActionCoroutine;

namespace MainEntry
{

    public class MainEntry : MonoBehaviour
    {
        [SerializeField] private Tick tick;
        [SerializeField] private DemodulatorController demodulatorController;
        [SerializeField] private SimpleActionCoroutine simpleActionCoroutine;
        [SerializeField] private AmplifaerBootstrap amplifaerBootstrap;
        [SerializeField] private SignalManager signalManager;
        [SerializeField] private DemodulatorBootstrap demodulatorBootstrap;
        [SerializeField] private DecoderBootstrap decoderBootstrap;
        [SerializeField] private DecoderController decoderController;
        [SerializeField] private CoderBootstrap coderBootstrap;
        [SerializeField] private SatelliteTunerBootstrap satelliteTunerBootstrap;
        [SerializeField] private SatelliteTunerController satelliteTunerController;
        [SerializeField] private NoiseGenerator noiseGenerator;
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private PelengatorController pelengatorController;
        [SerializeField] private SignalsTimer signalsTimer;
        [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private ShopBootstrap shopBootstrap;
        [SerializeField] private ManualSliderScroll manualSliderScroll;
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private BalanceSystem balanceSystem;
        [SerializeField] private MessageBootstrap messageBootstrap;
        [SerializeField] private LaserDeviceBootstrap laserDeviceBootstrap;
        [SerializeField] private LaserDeviceController laserDeviceController;
        [SerializeField] private EndingController endingController;
        [SerializeField] private TutorialController tutorialController;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private TutorialStepsMove tutorialStepsMove;

        private Context _context;
        private Injector _injector;

        private void Awake()
        {
            InitializeDependencyInjection();
        }

        private void InitializeDependencyInjection()
        {
            _context = new Context();
            _injector = new Injector(_context);
            _context.Install(_injector);
            RegisterGameComponents();
            _context.ApplyInstall();
        }

        private void RegisterGameComponents()
        {
            //installation here:
            _context.Install(tick);

            CoroutineSequenceRunner coroutineSequenceRunner = new CoroutineSequenceRunner();
            GameEventBuffer gameEventBuffer = new GameEventBuffer();

            _context.Install(coroutineSequenceRunner);

            _context.Install(gameEventBuffer);
            
            _context.Install(simpleActionCoroutine);

            _context.Install(signalManager);

            _context.Install(signalsTimer);

            _context.Install(demodulatorController);

            _context.Install(decoderController);

            _context.Install(amplifaerBootstrap);

            _context.Install(demodulatorBootstrap);

            _context.Install(decoderBootstrap);

            _context.Install(coderBootstrap);

            _context.Install(satelliteTunerBootstrap);

            _context.Install(satelliteTunerController);

            _context.Install(laserDeviceBootstrap);

            _context.Install(laserDeviceController);

            _context.Install(noiseGenerator);

            _context.Install(enemyController);

            _context.Install(pelengatorController);

            _context.Install(currencyManager);

            _context.Install(shopBootstrap);

            _context.Install(manualSliderScroll);

            _context.Install(shopManager);

            _context.Install(balanceSystem);

            _context.Install(messageBootstrap);

            _context.Install(endingController);

            _context.Install(tutorialController);

            _context.Install(playerController);

            _context.Install(tutorialStepsMove);



            _context.ApplyInstall();

            //initialization here:
            coroutineSequenceRunner.Initialize();
           
            signalManager.Initialize();
            signalsTimer.Initialize();
            amplifaerBootstrap.Initialize();
            demodulatorBootstrap.Initialized();
            demodulatorController.Initialize();
            decoderBootstrap.Initialize();
            decoderController.Initialize();
            coderBootstrap.Initialize();
            satelliteTunerBootstrap.Initialize();
            satelliteTunerController.Initialize();
            laserDeviceBootstrap.Initialize();
            laserDeviceController.Initialize();
            noiseGenerator.Initialize();
            enemyController.Initialize();
            pelengatorController.Initialize();
            manualSliderScroll.Initialize();
            currencyManager.Initialize();
            shopBootstrap.Initialize();
            shopManager.Initialize();
            balanceSystem.Initialize();
            messageBootstrap.Initialize();
            endingController.Initialize();
            tutorialController.Initialize();
            playerController.Initialize();
            tutorialStepsMove.Initialize();
        }


        private void OnDestroy()
        {
            _context?.Dispose();
        }
    }
}