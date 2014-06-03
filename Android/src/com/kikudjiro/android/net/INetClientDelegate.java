package com.kikudjiro.android.net;


/**
 * Interface of delegate that masters the TCP/IP layer for android application as a client.
 * 
 * @author Alexander Alexeychuk
 * @email kikudjiro@gmail.com
 */
public interface INetClientDelegate {
	void onConnect();
	void onConnectTimeout();
	void onDisconnect();
	void onReceive(String[] message);
	void onError(String message);
}
