﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoMAscii.Ascii
{
    public class Art
    {
        public static readonly string[] Logo = new string[] {
            @" __      __            .__       .___         _____     _____                               .__        ",
            @"/  \    /  \___________|  |    __| _/   _____/ ____\   /     \   ____   ______  _  _______  |__| ______",
            @"\   \/\/   /  _ \_  __ \  |   / __ |   /  _ \   __\   /  \ /  \ /  _ \ / ___\ \/ \/ /\__  \ |  |/  ___/",
            @" \        (  <_> )  | \/  |__/ /_/ |  (  <_> )  |    /    Y    (  <_> ) /_/  >     /  / __ \|  |\___ \ ",
            @"  \__/\  / \____/|__|  |____/\____ |   \____/|__|    \____|__  /\____/\___  / \/\_/  (____  /__/____  >",
            @"       \/                         \/                         \/      /_____/              \/        \/ ",
        };
        public static string[] Menu(string template) => new string[] {
            @"¬c+=====================================================================================================+§¬",
            //@"¬c_______________________________________________________________________________________________________§¬",
            @"¬c\_§¬CI§¬cnfo____/\_§¬CC§¬create__/\_§¬CB§¬cind____/\_§¬CS§¬cpend___/\_§¬CP§¬clay____/\_§¬C.§¬c...____/\_§<template         >¬c/\_[§¬CESC§¬c]_Exit__/§¬".Replace("<template         >", template),
           //@"¬c_______________________________________________________________________________________________________§¬",
            // @"¬c+----------+----------+----------+----------+----------+----------+---------------------+-------------+§¬"
        };

        public static string[] ColumnHeader = new string[]
        {
            @"¬B   \_Address__________________________/\_State_/\_Funds___/\_Name_______/\_Rating_/\_Level_/\_Gold___/§¬",
           // @"---+-----------------------------------+-------+----------+-------------+-------------------------------¬"
        };

        public static readonly string[] Trailer = new string[] {
            @"¬c+-----------------------------------------------------------------------------------------------------+§¬",
            @"¬c¦§ ¬CThis is a§ ¬Bblockchain§ ¬Cexperiment, not a§ ¬Bbuisnessplan§¬C! Enjoy the game!       .:Mogwaicoin Team 2018:. ¬c¦§¬",
            @"¬c+-----------------------------------------------------------------------------------------------------+§¬"
        };
    }
}
