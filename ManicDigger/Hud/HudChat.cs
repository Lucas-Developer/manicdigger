﻿using System;
using System.Collections.Generic;
using System.Text;
using ManicDigger.Renderers;
using System.Drawing;

namespace ManicDigger.Gui
{
    public class HudChat
    {
        [Inject]
        public IDraw2d draw2d;
        [Inject]
        public IViewportSize viewportsize;

        public bool IsTyping;
        public string GuiTypingBuffer;
        public float ChatFontSize = 12f;
        public int ChatScreenExpireTimeSeconds = 20;
        public int ChatLinesMaxToDraw = 10;
        public List<Chatline> ChatLines = new List<Chatline>();
        public int ChatPageScroll;
        public string Name { get { return "Chat"; } }

        public void Render()
        {
            if (draw2d != null)
            {
                DrawChatLines(IsTyping);
                if (IsTyping)
                {
                    DrawTypingBuffer();
                }
            }
        }
        public void AddChatline(string s)
        {
            ChatLines.Add(new Chatline() { text = s, time = DateTime.Now });
        }
        public void DrawChatLines(bool all)
        {
            /*
            if (chatlines.Count>0 && (DateTime.Now - chatlines[0].time).TotalSeconds > 10)
            {
                chatlines.RemoveAt(0);
            }
            */
            List<Chatline> chatlines2 = new List<Chatline>();
            if (!all)
            {
                foreach (Chatline c in ChatLines)
                {
                    if ((DateTime.Now - c.time).TotalSeconds < ChatScreenExpireTimeSeconds)
                    {
                        chatlines2.Add(c);
                    }
                }
            }
            else
            {
                int first = ChatLines.Count - ChatLinesMaxToDraw * (ChatPageScroll + 1);
                if (first < 0)
                {
                    first = 0;
                }
                int count = ChatLines.Count;
                if (count > ChatLinesMaxToDraw)
                {
                    count = ChatLinesMaxToDraw;
                }
                for (int i = first; i < first + count; i++)
                {
                    chatlines2.Add(ChatLines[i]);
                }
            }
            for (int i = 0; i < chatlines2.Count; i++)
            {
                draw2d.Draw2dText(chatlines2[i].text, 20, 90f + i * 25f, ChatFontSize, Color.White);
            }
            if (ChatPageScroll != 0)
            {
                draw2d.Draw2dText("Page: " + ChatPageScroll, 20, 90f + (-1) * 25f, ChatFontSize, Color.Gray);
            }
        }
        public void DrawTypingBuffer()
        {
            draw2d.Draw2dText(GuiTypingBuffer + "_", 50, viewportsize.Height - 100, ChatFontSize, Color.White);
        }
    }
    public class Chatline
    {
        public string text;
        public DateTime time;
    }
}
