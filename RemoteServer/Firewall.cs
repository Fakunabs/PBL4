using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


public static class FireWall
{
    public static string ProgramName = "";//Set name here if not using the main program

    public static bool AllowThisProgram(string Protocol, string RemotePorts, string LocalPorts, string Direction)
    {
        string ProgramFullName = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;//Will work even if debugging AppName.vshost.exe
        int End = ProgramFullName.LastIndexOf("\\");
        string programName = FireWall.ProgramName;
        if (FireWall.ProgramName.Length == 0) programName = ProgramFullName.Substring(End + 1);
        return AllowProgram(programName, ProgramFullName, Protocol, RemotePorts, LocalPorts, Direction);
    }


    public static bool AllowProgram(string programName, string ProgramFileName, string Protocol, string RemotePorts, string LocalPorts, string Direction)
    {
        //netsh advfirewall firewall delete rule name="NetBIOS TCP Port 139" protocol=TCP localport=139
        //netsh advfirewall firewall add rule name="NetBIOS TCP Port 139" dir=in action=allow protocol=TCP localport=139
        programName = "Allow " + programName + " " + Protocol.ToUpper() + " " + Direction.ToLower() + " " + LocalPorts + " " + RemotePorts;
        programName = programName.Replace("  ", " ").Trim();
        string CmdDelete = "netsh advfirewall firewall delete rule name='" + programName + "' protocol=" + Protocol.ToUpper() + " dir=" + Direction.ToLower();
        if (LocalPorts.Length > 0) CmdDelete += " localport=\"" + LocalPorts + "\"";
        if (RemotePorts.Length > 0) CmdDelete += " remoteport=\"" + RemotePorts + "\"";
        if (ProgramFileName.Length > 0) CmdDelete += " program=\"" + ProgramFileName + "\"";
        string Test = ExecuteCommandAsAdmin(CmdDelete);
        string CmdAdd = "netsh advfirewall firewall add rule name='" + programName + "' dir=" + Direction.ToLower() + " action=allow protocol=" + Protocol.ToUpper();
        if (LocalPorts.Length > 0) CmdAdd += " localport=\"" + LocalPorts + "\""; else LocalPorts = "Any";
        if (RemotePorts.Length > 0) CmdAdd += " remoteport=\"" + RemotePorts + "\""; else RemotePorts = "Any";
        if (ProgramFileName.Length > 0) CmdAdd += " program=\"" + ProgramFileName + "\""; else ProgramFileName = "Any";
        CmdAdd += " description='Allow " + ProgramFileName + " on " + Protocol + " using local-ports " + LocalPorts + " and remote-ports " + RemotePorts + "'";
        return ExecuteCommandAsAdmin(CmdAdd).ToUpper().StartsWith("OK");
    }

    public static string ExecuteCommandAsAdmin(string command)
    {
        ProcessStartInfo psinfo = new ProcessStartInfo();
        psinfo.FileName = "powershell.exe";
        psinfo.Arguments = command;
        psinfo.RedirectStandardError = true;
        psinfo.RedirectStandardOutput = true;
        psinfo.UseShellExecute = false;

        using (Process proc = new Process())
        {
            proc.StartInfo = psinfo;
            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();

            if (string.IsNullOrEmpty(output))
                output = proc.StandardError.ReadToEnd();

            return output;
        }
    }



    private static bool AddNewRule(string RuleName, string Description, string RemoteAddresses, string RemotePorts, string LocalPorts, string Protocol, string Action, string Direction, string ApplicationName, bool DontUpdate)
    {

        return false;
    }
}

