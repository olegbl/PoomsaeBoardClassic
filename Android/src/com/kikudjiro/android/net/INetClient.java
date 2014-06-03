package com.kikudjiro.android.net;

/**
 * Interface of the TCP/IP layer for android application as a client.
 * 
 * @author Alexander Alexeychuk
 * @email kikudjiro@gmail.com
 */
public interface INetClient {
	boolean connect(String host, int port);
	void disconnect();
	void send(byte[] data);
}
