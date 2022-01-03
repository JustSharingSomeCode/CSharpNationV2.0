using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CSharpNation.Config;
using CSharpNation.Tools;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para ConfigGUI.xaml
    /// </summary>
    public partial class ConfigGUI : UserControl
    {
        public ConfigGUI()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            AnalyzerSamplesTxt.Text = GlobalConfig.Lines.ToString();
            EnableLogoShakingTb.Value = GlobalConfig.EnableShaking;

            AutoBackgroundChangeTb.Value = GlobalConfig.AutoBackgroundChange;
            BackgroundDurationTxt.Text = GlobalConfig.BackgroundTime.ToString();
            BackgroundDimFa.Value = GlobalConfig.BackgroundDim * 100 / 255;
            BackgroundMovementTb.Value = GlobalConfig.BackgroundMovement;

            EnableGlowTb.Value = GlobalConfig.EnableGlow;
            GlowMaxAlphaTxt.Text = GlobalConfig.GlowMaxAlpha.ToString();
            GlowMaxAlphaAtSizeTxt.Text = GlobalConfig.GlowMaxAlphaAtSize.ToString();
            GlowSizeTxt.Text = GlobalConfig.GlowSize.ToString();
            EnableReplayBufferTb.Value = GlobalConfig.EnableReplayBuffer;
            ReplayBufferSizeTxt.Text = GlobalConfig.ReplayBufferSize.ToString();
        }

        private bool EnterKeyPressed(Key keyPressed)
        {
            return keyPressed == Key.Enter;
        }

        private void AnalyzerSamplesTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.Lines = Convert.ToInt32(AnalyzerSamplesTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    AnalyzerSamplesTxt.Text = GlobalConfig.Lines.ToString();
                }
            }
        }

        private void BackgroundDurationTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.BackgroundTime = Convert.ToInt32(BackgroundDurationTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    BackgroundDurationTxt.Text = GlobalConfig.BackgroundTime.ToString();
                }
            }
        }

        private void GlowMaxAlphaTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.GlowMaxAlpha = Convert.ToInt32(GlowMaxAlphaTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    GlowMaxAlphaTxt.Text = GlobalConfig.GlowMaxAlpha.ToString();
                }
            }
        }

        private void GlowMaxAlphaAtSizeTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.GlowMaxAlphaAtSize = Convert.ToInt32(GlowMaxAlphaAtSizeTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    GlowMaxAlphaAtSizeTxt.Text = GlobalConfig.GlowMaxAlphaAtSize.ToString();
                }
            }
        }

        private void GlowSizeTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.GlowSize = Convert.ToInt32(GlowSizeTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    GlowSizeTxt.Text = GlobalConfig.GlowSize.ToString();
                }
            }
        }

        private void ReplayBufferSizeTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterKeyPressed(e.Key))
            {
                try
                {
                    GlobalConfig.ReplayBufferSize = Convert.ToInt32(ReplayBufferSizeTxt.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.AddError(new Error(Error.Type.CriticalError, ex.Message));
                    ReplayBufferSizeTxt.Text = GlobalConfig.ReplayBufferSize.ToString();
                }
            }
        }

        private void EnableLogoShakingTb_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.EnableShaking = EnableLogoShakingTb.Value;
        }

        private void AutoBackgroundChangeTb_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.AutoBackgroundChange = AutoBackgroundChangeTb.Value;
        }

        private void BackgroundMovementTb_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.BackgroundMovement = BackgroundMovementTb.Value;
        }

        private void EnableGlowTb_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.EnableGlow = EnableGlowTb.Value;
        }

        private void EnableReplayBufferTb_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.EnableReplayBuffer = EnableReplayBufferTb.Value;
        }

        private void BackgroundDimFa_OnValueChanged(object sender, EventArgs e)
        {
            GlobalConfig.BackgroundDim = (int)(BackgroundDimFa.Value / 100 * 255);
        }
    }
}
