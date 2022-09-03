using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsRPC;

namespace ETWPlayGround
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TraceEventSession m_TraceSession;

            using (var session = new TraceEventSession("MySimpleSession"))
            {
                Console.CancelKeyPress += delegate
                {
                    session.Source.StopProcessing();
                    session.Dispose();
                };


                m_TraceSession = session;

                session.EnableProvider("Microsoft-Windows-RPC", Microsoft.Diagnostics.Tracing.TraceEventLevel.Verbose);
                var parser = new MicrosoftWindowsRPCTraceEventParser(session.Source);

                // Do we want to include more events? server events?

                parser.All += e2 =>
                {
                    // addEventToListView(e2);
                    Console.WriteLine($"{e2.ProcessID} eventID = {e2.ID} PID = {e2.ProcessID} TID={e2.ThreadID} Level={e2.Level} opCode={e2.Opcode} taskName={e2.Task}");
                    Console.Write("\t");
                    
                    /* 
                      // Throws an error "Cross-thread operation not valid: Control 'textBox1' accessed from a thread other than the thread it was created on."
                     string funcName = getFunctionName(e2.InterfaceUuid.ToString(), e2.ProcNum);
                     ListViewItem item = new ListViewItem(e2.ProcessID.ToString());
                     item.SubItems.Add(e2.ThreadID.ToString());
                     item.SubItems.Add(e2.InterfaceUuid.ToString());
                     item.SubItems.Add(funcName);
                     listView1.Items.Add(item);*/
                    // Console.WriteLine($"{e2.ID}                {funcName}");
                };
                session.Source.Process();
            }

        }
    }
}
