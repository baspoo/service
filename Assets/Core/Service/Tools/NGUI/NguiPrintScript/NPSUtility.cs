﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace NPS.Utls
{
    public class NPSUtility : MonoBehaviour
    {

        static NPSUtility m_instance;
        public static NPSUtility instance
        {
            get
            {
                if (m_instance == null) m_instance = ((GameObject)Resources.Load("NGUIservice/NguiPrintScript")).GetComponent<NPSUtility>();
                return m_instance;
            }
        }
        public Texture Cover;
        public List<Font> Fonts;
        public List<Texture> Texture;

    }

}

