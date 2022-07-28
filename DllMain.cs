using System;
using MMVis;
using MMVis.Dev;
using OpenTK.Graphics.OpenGL;

[assembly: MMVis.Dev.DFBMod(DFBModAttribute.ModType.DFB_VISUALIZER, typeof(MMDemoVis.DemoVis))]

namespace MMDemoVis
{
    public class DemoVis : MMVis.Dev.INoteVisualizer
    {
        float[] vbo;
        float[] cbo;
        int maxindex = 128000;
        int index = 0;

        float ax = 1.0F;
        float ay = 1.0F;

        bool INoteVisualizer.Initialize(IMM mmi)
        {
            vbo = new float[maxindex * 6 * 3];
            cbo = new float[maxindex * 6 * 3];

            return true;
        }

        void INoteVisualizer.Finalize(IMM mmi)
        {

        }

        private void FlushToilet()
        {
            if(index == 0)
                return;

            GL.DrawArrays(PrimitiveType.Triangles, 0, index * 6);
            GL.Flush();
            index = 0;
        }

        private void DrawNote(float x, float y, float w, float b, int col1, int col2)
        {
            bool tall = b - y > (ay * 2);

            if(tall ? (index+1 >= maxindex) : index == maxindex)
            {
                FlushToilet();
                index = 0;
            }

            float cr = (byte)col1 * (1 / 256.0F);
            float cg = (byte)(col1 >> 8) * (1 / 256.0F);
            float cb = (byte)(col1 >> 16) * (1 / 256.0F);

            float gr = cr * 0.2F;
            float gg = cg * 0.2F;
            float gb = cb * 0.2F;

            float dr = (byte)col2 * (1 / 256.0F);
            float dg = (byte)(col2 >> 8) * (1 / 256.0F);
            float db = (byte)(col2 >> 16) * (1 / 256.0F);

            int i = index * 18;
            if(tall)
                index += 2;
            else
                index++;

            vbo[i + 0] = x; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;
            i += 3;

            vbo[i + 0] = x; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;
            i += 3;

            vbo[i + 0] = x + w; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;
            i += 3;

            vbo[i + 0] = x + w; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;
            i += 3;

            vbo[i + 0] = x + w; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;
            i += 3;

            vbo[i + 0] = x; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
            cbo[i + 0] = gr; cbo[i + 1] = gg; cbo[i + 2] = gb;

            if(tall)
            {
                i += 3;

                x += ax;
                w -= ax;
                y += ay;
                b -= ay;

                vbo[i + 0] = x; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
                cbo[i + 0] = cr; cbo[i + 1] = cg; cbo[i + 2] = cb;
                i += 3;

                vbo[i + 0] = x; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
                cbo[i + 0] = cr; cbo[i + 1] = cg; cbo[i + 2] = cb;
                i += 3;

                vbo[i + 0] = x + w; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
                cbo[i + 0] = dr; cbo[i + 1] = dg; cbo[i + 2] = db;
                i += 3;

                vbo[i + 0] = x + w; vbo[i + 1] = b; vbo[i + 2] = 0.0F;
                cbo[i + 0] = dr; cbo[i + 1] = dg; cbo[i + 2] = db;
                i += 3;

                vbo[i + 0] = x + w; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
                cbo[i + 0] = dr; cbo[i + 1] = dg; cbo[i + 2] = db;
                i += 3;

                vbo[i + 0] = x; vbo[i + 1] = y; vbo[i + 2] = 0.0F;
                cbo[i + 0] = cr; cbo[i + 1] = cg; cbo[i + 2] = cb;
            }
        }

        ulong INoteVisualizer.DrawNotes(IMM mmi, FastList<MMNote>[] ChannelNotes, ulong PlayHead, ulong RenderHead)
        {
            ulong NOTE_SCREENTIME = mmi.ScreenTime;
            ulong MAX_NOTE_CHN = mmi.ColumnLimit;
            ulong MAX_NOTE_DRAW = mmi.NoteLimit;
            ulong RealTime = PlayHead;

            int[] colortable = mmi.NoteColor;
            int[] colorgrad = mmi.NoteGradient;

            ax = 256.0F / (float)mmi.Width;
            ay = 2.0F / (float)mmi.Height;

            double scale = 1.0 / NOTE_SCREENTIME;

            ulong VisBot = RenderHead;
            ulong VisTop = VisBot + NOTE_SCREENTIME; // up is +time

            ulong totalcount = 0;

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);

            GL.ColorPointer(3, ColorPointerType.Float, 0, cbo);
            GL.VertexPointer(3, VertexPointerType.Float, 0, vbo);

            for(int slot = 0; slot != ChannelNotes.Length; slot++)
            {
                var list = ChannelNotes[slot];

                ulong chncount = 0;

                float notex = slot;
                float notew = 1;

                var it = list.Iterate();
                while(it.MoveNext(out var mn))
                {
                    if(mn.ontime > VisTop) //ontime is above screen
                        continue;

                    if(mn.offtime == ulong.MaxValue) // note is on
                    {
                        float bottom = 1.0F; // clamp bottom
                        if(mn.ontime > VisBot) // didn't reach the bottom yet
                            bottom = (float)((VisTop - mn.ontime) * scale);

                        //top is always 0
                        DrawNote(notex, 0.0F, notew, bottom, colortable[mn.colorindex], colorgrad[mn.colorindex]);
                        chncount++;
                    }
                    else if(mn.offtime > VisBot) // note is off, but not yet off-screen
                    {
                        float top = 0.0F;
                        if(mn.offtime <= VisTop)
                            top = (float)((VisTop - mn.offtime) * scale);

                        float bottom = 1.0F; // clamp bottom
                        if(mn.ontime > VisBot) // didn't reach the bottom yet
                            bottom = (float)((VisTop - mn.ontime) * scale);

                        DrawNote(notex, top, notew, bottom, colortable[mn.colorindex], colorgrad[mn.colorindex]);
                        chncount++;
                    }

                    if(chncount >= MAX_NOTE_CHN)
                        break;

                    if((totalcount + chncount) >= MAX_NOTE_DRAW)
                        break;
                }

                totalcount += chncount;

                if(totalcount >= MAX_NOTE_DRAW)
                    break;
            }

            FlushToilet();

            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.VertexArray);

            return totalcount;
        }
        
        void INoteVisualizer.DrawKeys(IMM mmi)
        {

        }
    }
}
