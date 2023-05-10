using sxr_internal;
using UnityEngine;

namespace SampleExperimentScene
{
    public class ExperimentScript : MonoBehaviour
    {
        private int numHits;
        private int guessedNumber;

        void Update()
        {
            switch (sxr.GetPhase())
            {
                case 0: // Start Screen Phase
                    sxr.DisplayImage("loading", sxr_internal.UI_Position.FullScreen1);
                    break;

                case 1: // Instruction Phase
                    switch (sxr.GetStepInTrial())
                    {
                        case 0:
                            sxr.HideImagesUI();
                            sxr.DisplayText("In this experiment, you will try to put a ball in the "
                                            + "target box as many times as possible in ten seconds. (Trigger to continue)");

                            if (sxr.GetTrigger())
                            {
                                sxr.WriteHeaderToTaggedFile("mainFile", "numHits");
                                sxr.HideAllText();
                                sxr.NextStep();
                            }

                            break;

                        case 1:
                            sxr.DisplayImage("practiceStart");
                            if (sxr.GetTrigger())
                            {
                                sxr.HideImagesUI();
                                sxr.NextPhase();
                            }

                            break;
                    }

                    break;

                case 2: // Practice Round
                case 3: // Testing Round
                    sxr.DisplayText(GazeHandler.Instance.GetScreenFixationPoint().x + ", " +
                                    GazeHandler.Instance.GetScreenFixationPoint());
                    switch (sxr.GetStepInTrial())
                    {
                        case 0: // Hit trigger to start
                            sxr.DisplayImage("trigger");
                            if (sxr.GetTrigger())
                            {
                                sxr.HideImagesUI();
                                sxr.NextStep();
                                sxr.StartTimer(10);
                                if (sxr.GetPhase() == 3)
                                    sxr.StartRecordingCameraPos(false);
                            }

                            break;

                        case 1: // Make sphere/move sphere back to start
                            if (!sxr.ObjectExists("Sphere"))
                            {
                                sxr.SpawnObject(PrimitiveType.Sphere, "Sphere", 1, 1.5f, 0);
                                sxr.MakeObjectGrabbable("Sphere");
                                sxr.EnableObjectPhysics("Sphere", false);
                            }
                            else
                                sxr.MoveObjectTo("Sphere", 1, 1.5f, 0);

                            sxr.NextStep();
                            break;

                        case 2: // Runs until CheckTimer()==10 -- Looking for sphere in box
                            if (sxr.CheckTimer())
                            {
                                sxr.NextPhase();
                                sxr.ChangeExperimenterTextbox(4, "Number of goals: " + numHits);
                                if (sxr.GetPhase() == 3)
                                {
                                    sxr.PauseRecordingCameraPos();
                                    sxr.WriteToTaggedFile("mainFile", numHits.ToString());
                                }
                            }

                            if (sxr.CheckCollision("Sphere", "TargetBox"))
                            {
                                numHits++;
                                sxr.PlaySound(sxr_internal.ProvidedSounds.Ding);
                                sxr.MoveObjectTo("Sphere", 1, 1.5f, 0, 0);
                            }

                            break;
                    }

                    break;

                case 4: // Input from user
                    sxr.InputSlider(0, 20, "How many times do you think you hit the goal? [" + guessedNumber + "]",
                        true);
                    if (sxr.ParseInputUI(out guessedNumber))
                        sxr.NextPhase();
                    break;

                case 5: // Finished
                    sxr.DisplayImage("finished");
                    break;
            }
        }
    }
}