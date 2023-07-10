# SmartFlappyBird
 Making Flappy Bird great again.
 This is a clone of the Flappy bird game, with Reinforcement learning to allow auto-play.
 
 # Environment
* Windows 10 Home x64
* Unity 2019.4.14f1
* Recommended to create a virtual environment for managing Python versions and modules: https://github.com/Unity-Technologies/ml-agents/blob/release_10_branch/docs/Using-Virtual-Environment.md

### Installation
Reference here: https://github.com/Unity-Technologies/ml-agents/blob/release_10_branch/docs/Installation.md
* ML agents 1.0.6 (Installed from Unity package manager)
* Python 3.7.4 (in env)
* mlagents from PyPi (in env)

### Setup
* The basic guide : https://github.com/Unity-Technologies/ml-agents/blob/release_10_branch/docs/Learning-Environment-Create-New.md
* and also : https://github.com/Unity-Technologies/ml-agents/blob/release_10_branch/docs/Getting-Started.md
* For the flappy bird implementation: https://becominghuman.ai/teaching-ai-to-play-flappy-bird-with-unity-70f7b661663d

# Usage
## Training
* Activate the python environment. Make sure you can run ```mlagents-learn --help```
* Assume we're at the folder where we cloned Unity ML-Agents Toolkit repository, run ```mlagents-learn {path to yaml file} --run-id={anything}```
* Example : ```mlagents-learn config/ppo/SmartFlappyBirdyaml --run-id=SmartFlappyBird```
* On the player agent's Behavior Parameters script, make sure Behavior Type is set to Default
* When the message "Start training by pressing the Play button in the Unity Editor" is displayed on the screen, you can press the Play button in Unity to start training in the Editor

Example of a config file, this is the current one used:
```behaviors:
  SmartFlappyBird:
    trainer_type: ppo
    hyperparameters:
      batch_size: 32
      buffer_size: 256
      learning_rate: 0.0003
      beta: 0.005
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 256
      num_layers: 3
      vis_encode_type: simple
    reward_signals:
      extrinsic:
        gamma: 0.9
        strength: 1.0
    keep_checkpoints: 5
    max_steps: 5000000
    time_horizon: 3
    summary_freq: 2000
    threaded: true
```
After running once, the training results will be in the folder ```/results/{run_id}```
If you need to rerun the training, first delete the ```results/{run_id}``` folder.

# Visualizing training with Tensorboard
Run ```tensorboard --logdir /results/SmartFlappyBird```
Past experiments:
* Baseline with the above config. https://tensorboard.dev/experiment/Z8hRCaP3S4OVRFQoP69nLg/#scalars


