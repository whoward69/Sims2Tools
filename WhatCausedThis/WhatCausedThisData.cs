/*
 * What Caused This - a utility for reading The Sims 2 object error logs and determining which package file(s) caused it
 *                  - see http://www.picknmixmods.com/Sims2/Notes/WhatCausedThis/WhatCausedThis.html
 *
 * Sims2Tools - a toolkit for manipulating The Sims 2 DBPF files
 *
 * William Howard - 2020-2025
 *
 * Permission granted to use this code in any way, except to claim it as your own or sell it
 */

using System;
using System.Data;

namespace WhatCausedThis
{
    [System.ComponentModel.DesignerCategory("")]
    class WhatCausedThisData : DataTable
    {
        private readonly DataColumn colPackage = new DataColumn("Hack Package", typeof(string));
        private readonly DataColumn colScore = new DataColumn("Hack Score", typeof(int));

        public WhatCausedThisData()
        {
            this.Columns.Add(colPackage);
            this.Columns.Add(colScore);
        }

        public void Add(string package)
        {
            int score = Convert.ToInt32(package.Substring(0, 1));
            string name = package.Substring(1);
            string suffix = score == 1 ? "secondary possible" : score == 2 ? "secondary likely" : score == 3 ? "possible" : "likely";

            this.Rows.Add($"{name} ({suffix})", score);
        }
    }
}
