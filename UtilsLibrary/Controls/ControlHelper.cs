/*
 * Repository Wizard - a utility for repositorying clothes/objects to another item (also known as master/slave technique)
 *                   - see http://www.picknmixmods.com/Sims2/Notes/RepositoryWizard/RepositoryWizard.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2026
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System.Windows.Forms;

namespace Sims2Tools.Controls
{
    public static class ControlHelper
    {
        public static void SetDropDownWidth(ComboBox comboBox)
        {
            int maxWidth = 0;

            foreach (var entry in comboBox.Items)
            {
                int width = TextRenderer.MeasureText(comboBox.GetItemText(entry), comboBox.Font).Width;

                if (width > maxWidth) maxWidth = width;
            }

            if (comboBox.Items.Count > comboBox.MaxDropDownItems)
            {
                maxWidth += SystemInformation.VerticalScrollBarWidth;
            }

            if (maxWidth > comboBox.Width)
            {
                comboBox.DropDownWidth = maxWidth;
            }
        }
    }
}
