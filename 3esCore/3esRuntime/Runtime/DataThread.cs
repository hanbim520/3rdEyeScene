﻿using Tes.Collections;
using Tes;
using Tes.IO;
using Tes.Net;

namespace Tes.Main
{
  /// <summary>
  /// Base class for thread objects used as message sources.
  /// </summary>
  /// <remarks>
  /// A data thread is responsible for reading incoming data, generally over a
  /// network connection or from file, and pushing data packets into the
  /// <see cref="PacketQueue"/>. The owner of the <see cref="DataThread"/>
  /// pops the packet queue and processes the results.
  /// 
  /// The owner can request the <see cref="DataThread"/> to perform particular
  /// operations such as:
  /// <list type="bullet">
  /// <item>Set the <see cref="CurrentFrame"/> number</item>
  /// <item>Toggle the paused <see cref="Paused"/></item>
  /// </list>
  /// 
  /// These operations are not supported on live threads and will be ignored
  /// if <see cref="IsLiveStream"/> is true.
  /// 
  /// For recorded streams, it is up to the <see cref="DataThread"/>
  /// implementation to maintain the correct packet timing.
  /// </remarks>
  public abstract class DataThread
  {
    /// <summary>
    /// Access the thread safe pending packet queue.
    /// </summary>
    public abstract Queue<PacketBuffer> PacketQueue { get; }
    /// <summary>
    /// Request or set the current frame number.
    /// </summary>
    public abstract uint CurrentFrame { get; set; }
    /// <summary>
    /// Request the total number of frames. Will change for live streams.
    /// </summary>
    public abstract uint TotalFrames { get; set; }
    /// <summary>
    /// Frame value to play up to and stop. Not for live streams.
    /// </summary>
    public abstract uint TargetFrame { get; set; }
    /// <summary>
    /// Is this a live stream (<c>true</c>) or recorded (<c>false</c>)
    /// </summary>
    public abstract bool IsLiveStream { get; }
    /// <summary>
    /// Has the thread been started?
    /// </summary>
    public abstract bool Started { get; }
    /// <summary>
    /// Get/set the pause state of the thread (not for live threads).
    /// </summary>
    public abstract bool Paused { get; set; }
    /// <summary>
    /// Is the thread currently catchup up to the desired frame?
    /// </summary>
    /// <remarks>
    /// This is principally for playback threads when attempting to process data
    /// for large frame deltas such as when scrubbing.
    /// </remarks>
    public abstract bool CatchingUp { get; }
    /// <summary>
    /// Start the thread.
    /// </summary>
    /// <returns>True on success.</returns>
    public abstract bool Start();
    /// <summary>
    /// Join the thread, exiting the thread loop.
    /// </summary>
    /// <returns>True on success.</returns>
    public abstract bool Join();
    /// <summary>
    /// Request the thread look stop looping.
    /// </summary>
    public abstract void Quit();

    /// <summary>
    /// Create a reset packet.
    /// </summary>
    protected PacketBuffer BuildResetPacket()
    {
      PacketBuffer packet = new PacketBuffer(PacketHeader.Size + 32);
      ControlMessage message = new ControlMessage();

      // Write reset
      message.ControlFlags = 0;
      message.Value32 = 0;
      message.Value64 = 0;

      packet.Reset((ushort)RoutingID.Control, (ushort)ControlMessageID.Reset);
      message.Write(packet);
      packet.FinalisePacket();

      return packet;
    }
  }
}
