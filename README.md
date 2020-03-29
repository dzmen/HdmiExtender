# HdmiExtender

Captures the output of a Lenkeng LKV373 HDMI Extender TX (Sender) device, making the audio and video streams usable by 3rd party media players.

## Credits

This HdmiExtender project is based on the reverse-engineering efforts of danman, which he detailed here, in his blog: https://danman.eu/blog/reverse-engineering-lenkeng-hdmi-over-ip-extender/  His detailed descriptions were easy to follow, and led to a relatively pain-free implementation.  For full details of how the communication protocol works, check his blog!

It turns out that .NET's sockets are not quite low-level enough to read the full length of the malformed UDP packets generated by a Sender device.  So this project uses Pcap.NET to sniff the packets in their entirety.  You can find Pcap.NET here: https://github.com/PcapDotNet/Pcap.Net

## Who is it for?

This project is intended for *computer literate* people, preferably with C# programming experience, who want to capture video and audio data from a [Lenkeng LKV373 HDMI Extender TX (Sender) device](https://github.com/bp2008/HdmiExtender/wiki/Lenkeng-LKV373-HDMI-Extender-TX-%28Sender%29-device).

## Requirements

* One [Lenkeng LKV373 HDMI Extender TX (Sender) device](https://github.com/bp2008/HdmiExtender/wiki/Lenkeng-LKV373-HDMI-Extender-TX-%28Sender%29-device).  This was designed for the model with "V2.0" printed on the back, but it may work with the older model too.
* A Windows PC **with WinPCap installed**
* (Optional; Recommended) A second Network Interface adapter for the PC, so you can isolate the HDMI Extender Sender device from your LAN.  This will keep the video **broadcast** from the Sender device under control.
* (Optional; Recommended) A fast CPU for real-time transcoding of the video data using FFMPEG.

I have prepared some [ffmpeg sample configurations](https://github.com/bp2008/HdmiExtender/wiki/ffmpeg-sample-configurations) to demonstrate some of the ways to consume the video and audio streams produced by HdmiExtender.

## How to run

1. Download and install WinPCap from [https://www.winpcap.org/](https://www.winpcap.org/)
2. Download and install Visual Studio 2013 or higher
3. Open this project in Visual Studio 2013 or higher
4. Compile the source
5. Open CMD or Powershell
6. Navigate to the compile directory "debug" or "release"
7. You are able to run the compiled code in two ways:
	1. As a service by installing the application in the Windows service list. You do this by running the following command: "C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe" ".\HdmiExtenderService.exe"
	2. As just an application by running the following command: .\HdmiExtenderService.exe cmd <static IP of networkadapter> <IP of HMDI sender> <port local webserver>
	
## Other thoughts

This project is currently very unpolished.  I have not built a binary distribution because the network settings are hard-coded.  You will need to download the source code, open it in Visual Studio (2013 community edition is what I recommend), and modify the network settings before building.

## Disclaimer

The Lenkeng HDMI extender is not designed to be an HDMI capture device.  Seriously, you should probably buy a real HDMI capture device if that is what you need.  It will cost more, but it should work better.

Here are just some deficiencies I have noticed:

* You get no support from the manufacturer for this type of usage.
* Audio gets out of sync with the video.  Even with manual sync correction, it will often go out of sync again within minutes.  This is only tested using FFMPEG to combine and transcode the streams.  It is possible another method exists to keep the streams in sync, but I do not know of it.
* Surround sound gets downsampled to stereo.
* The JPEG images are encoded using the 16-235 "TV" range of luminance instead of the full 0-255 range, so you have reduced contrast in the output. (it can be corrected, with a bit of quality loss, using 3rd party tools or video players) 
* A high input refresh rate can cause tearing.  For example 60hz video from my laptop causes very bad tearing in the output from the Sender device, but with my Fire TV stick, I see no tearing.
* When active, the sender **broadcasts** between 30 and 90 Mbps depending on the input, so it may be ideal to isolate it from your network by using a second network adapter (I use a USB 3.0 network adapter).
