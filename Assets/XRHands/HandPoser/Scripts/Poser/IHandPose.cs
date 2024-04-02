// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

namespace InteractionsToolkit.Poser
{
    public interface IHandPose
    {
        public PoseData PrimaryPose { get; set; }
        public PoseData SecondaryPose { get; set; }

    }
}
