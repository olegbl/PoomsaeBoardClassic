package net.lokhvitsky.oleg.PoomsaeBoard;

import java.text.DecimalFormat;
import java.util.LinkedList;
import java.util.List;

import com.kikudjiro.android.net.INetClientDelegate;
import com.kikudjiro.android.net.NetClient;
import com.kikudjiro.android.net.NetClient.State;

import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.SeekBar;
import android.widget.TextView;
import android.widget.Toast;

public class PoomsaeBoardActivity extends Activity implements INetClientDelegate {
	public final DecimalFormat formatter = new DecimalFormat("#.#");
	
	protected NetClient client;
	protected EditText host, port, passphrase;
	protected TextView name, ring, judge, technical, presentation;
	protected LinearLayout connected, disconnected, center;
	protected Button connect, minorDeduction, majorDeduction, undoMinor, undoMajor;
	
	protected List<ScoreScale> scales;
	
	protected double min = 0.0, max = 0.0, minor = 0.0, major = 0.0;
	protected double scoret = 0.0;
	protected String names = "", poomsaes = "";
    
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        try
    	{
	        setContentView(R.layout.main);
	        
	        // Initialize Variables
	        this.disconnected = (LinearLayout) this.findViewById(R.id.disconnected);
	        this.host = (EditText) this.findViewById(R.id.host);
	        this.port = (EditText) this.findViewById(R.id.port);
	        this.passphrase = (EditText) this.findViewById(R.id.passphrase);
	        this.connect = (Button) this.findViewById(R.id.connect);
	        
	        this.connected = (LinearLayout) this.findViewById(R.id.connected);
	        this.center = (LinearLayout) this.findViewById(R.id.center);
	        this.name = (TextView) this.findViewById(R.id.athlete_name);
	        this.ring = (TextView) this.findViewById(R.id.ring_label);
	        this.judge = (TextView) this.findViewById(R.id.judge_label);
	        this.technical = (TextView) this.findViewById(R.id.technical);
	        this.presentation = (TextView) this.findViewById(R.id.presentation);
	        this.minorDeduction = (Button) this.findViewById(R.id.minorDeduction);
	        this.majorDeduction = (Button) this.findViewById(R.id.majorDeduction);
	        this.undoMinor = (Button) this.findViewById(R.id.undoMinor);
	        this.undoMajor = (Button) this.findViewById(R.id.undoMajor);
	        
	        this.scales = new LinkedList<ScoreScale>();
    	}
    	catch (Exception e)
    	{
    		Log.e("PoomsaeBoard", "Error: " + e.toString());
    		e.printStackTrace();
    	}
        
        // Initialize Listeners
        final PoomsaeBoardActivity me = this;
        
        this.connect.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				me.connect();
			}
        });
        
        this.minorDeduction.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				me.scoret -= me.minor;
				if (me.scoret < me.min) me.scoret = me.min;
				me.updateScores();
			}
        });
        
        this.majorDeduction.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				me.scoret -= me.major;
				if (me.scoret < me.min) me.scoret = me.min;
				me.updateScores();
			}
        });
        
        this.undoMinor.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				me.scoret += me.minor;
				if (me.scoret > me.max) me.scoret = me.max;
				me.updateScores();
			}
        });
        
        this.undoMajor.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				me.scoret += me.major;
				if (me.scoret > me.max) me.scoret = me.max;
				me.updateScores();
			}
        });
    }
    
    @Override
    public void onConfigurationChanged(Configuration newConfig) {
      super.onConfigurationChanged(newConfig);
    }
    
    public void updateScores() {
    	double presentationScore = 0.0;
    	for (ScoreScale scale : this.scales)
    		presentationScore += scale.getScore();
    	
    	this.technical.setText(formatter.format(this.scoret));
    	this.presentation.setText(formatter.format(presentationScore));
    	
    	this.client.send("technical", this.technical.getText().toString());
    	this.client.send("presentation", this.presentation.getText().toString());
    }
    
    public void connect() {
    	if (this.client != null && this.client.getState() == State.ONLINE) return;
    	
		String host = this.host.getText().toString();
		int port = Integer.parseInt(this.port.getText().toString());
		
		this.connected.setVisibility(View.GONE);
		this.disconnected.setVisibility(View.GONE);
		
		this.client = new NetClient(this);
		this.client.connect(host, port);
		
		return;
    }
    
    public void disconnect() {
    	if (this.client == null || this.client.getState() != NetClient.State.ONLINE) return;
    	this.client.send("disconnect");
    	this.client.disconnect();
    }
    
    private String joinString(String[] strings, String delimiter) {
    	String result = "";
    	for (int i = 0; i < strings.length; i ++) {
    		if (i != 0) result += delimiter;
    		result += strings[i];
    	}
    	return result;
    }

	@Override
	public void onConnect() {
		// Authenticate
		String passphrase = this.passphrase.getText().toString();
		this.client.send("register", passphrase);
	}
	
	@Override
	public void onDisconnect() {
		this.connected.setVisibility(View.GONE);
		this.disconnected.setVisibility(View.VISIBLE);
		
		this.toast("Disconnected");
	}

	@Override
	public void onConnectTimeout() {
		this.connected.setVisibility(View.GONE);
		this.disconnected.setVisibility(View.VISIBLE);
		
		this.toast("Connection Timed Out");
	}
	
	@Override
	public void onError(String message) {
		this.toast("Error: " + message);
	}
	
	protected void updateInfo() {
		this.name.setText(this.names + " : Poomsae " + this.poomsaes);
	}
	
	public class ScoreScale extends LinearLayout {
		public LinearLayout text;
		public TextView label, score;
		public SeekBar bar;
		
		public double min = 0.0;
		public double max = 0.0;
		public double step = 0.0;
		
		public ScoreScale(Context context) {
			super(context);
		}
		
		public ScoreScale(Context context, AttributeSet attrs) {
			super(context, attrs);
		}
		
		public double getScore() {
    		return ((double) this.bar.getProgress()) * this.step + this.min;
		}
		
		public void setScore(double score) {
			this.bar.setProgress((int)((score - this.min) / this.step));
			this.updateScore();
		}
		
		public void updateScore() {
			this.score.setText(formatter.format(this.getScore()));
		}
	}

	@Override
	public void onReceive(String[] message) {
		//this.toast("Message: " + this.joinString(message, "|"));
		final PoomsaeBoardActivity me = this;
		
		if (message.length >= 2 && message[0].startsWith("registered")) {
			this.connected.setVisibility(View.VISIBLE);
			this.disconnected.setVisibility(View.GONE);
			
			this.judge.setText("Judge # " + message[1]);
			
			this.client.send("query", "ring");
			this.client.send("query", "name");
			this.client.send("query", "poomsae");
		} else if (message.length >= 2 && message[0].startsWith("rejected")) {
			this.connected.setVisibility(View.GONE);
			this.disconnected.setVisibility(View.VISIBLE);
			
			this.toast("Connection Rejected!\n" + message[1]);
		} else if (message.length >= 2 && message[0].startsWith("name")) {
			this.names = message[1];
			this.updateInfo();
		} else if (message.length >= 2 && message[0].startsWith("ring")) {
			this.ring.setText("Ring # " + message[1]);
		} else if (message.length >= 2 && message[0].startsWith("poomsae")) {
			this.poomsaes = message[1];
			this.updateInfo();
		} else if (message.length >= 1 && message[0].startsWith("clear")) {
			for (ScoreScale scale : this.scales)
				scale.setScore(scale.max);
			this.scoret = this.max;
			this.updateScores();
		} else if (message.length >= 2 && message[0].startsWith("rules")) {
			this.min = Double.parseDouble(message[1]);
			this.max = Double.parseDouble(message[2]);
			this.minor = Double.parseDouble(message[3]);
			this.major = Double.parseDouble(message[4]);
			int length = (message.length - 5) / 4;
			
			for (ScoreScale scale : this.scales)
				this.center.removeView(scale);
			
			this.scales = new LinkedList<ScoreScale>();
			
			for (int i = 0; i < length; i ++) {
				String name = message[5 + i * 4 + 0];
				final double min = Double.parseDouble(message[5 + i * 4 + 1]);
				final double max = Double.parseDouble(message[5 + i * 4 + 2]);
				final double step = Double.parseDouble(message[5 + i * 4 + 3]);
				
		        final ScoreScale scale = new ScoreScale(this);
		        scale.setOrientation(LinearLayout.VERTICAL);
		        scale.setLayoutParams(new LayoutParams(LayoutParams.FILL_PARENT, LayoutParams.WRAP_CONTENT));
		        scale.setWeightSum((float)length * 0.3f);
		        
		        final LinearLayout text = new LinearLayout(this);
		        text.setOrientation(LinearLayout.HORIZONTAL);
		        text.setLayoutParams(new LinearLayout.LayoutParams(LayoutParams.FILL_PARENT, 0, 0.1f));
		        scale.addView(text);
		        
		        final TextView label = new TextView(this);
		        label.setLayoutParams(new LinearLayout.LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
		        label.setText(name);
		        text.addView(label);
		        
		        final TextView score = new TextView(this);
		        score.setLayoutParams(new LinearLayout.LayoutParams(LayoutParams.FILL_PARENT, LayoutParams.WRAP_CONTENT));
		        score.setGravity(Gravity.RIGHT);
		        text.addView(score);
		        
		        final SeekBar bar = new SeekBar(this);
		        bar.setLayoutParams(new LinearLayout.LayoutParams(LayoutParams.FILL_PARENT, 0, 0.2f));
		        bar.setMax((int)((max - min) / step));
		        bar.setProgress(0);
		        scale.addView(bar);
		        
		        bar.setOnTouchListener(new OnTouchListener() {
		        	final ScoreScale myscale = scale;
					@Override
					public boolean onTouch(View v, MotionEvent event) {
						myscale.updateScore();
						me.updateScores();
						return false;
					}
		        });
		        
		        this.center.addView(scale);
		        this.scales.add(scale);
		        
		        scale.min = min;
		        scale.max = max;
		        scale.step = step;
		        scale.text = text;
		        scale.label = label;
		        scale.score = score;
		        scale.bar = bar;
		        
		        scale.updateScore();
			}
		}
	}
	
	private void toast(final String message) {
		final Activity activity = this;
		runOnUiThread(new Runnable() {
		    public void run() {
		    	Toast.makeText(activity, message, Toast.LENGTH_SHORT).show();
		    }
		});
	}
	
	@Override
    public void onPause()
    {
        super.onPause();
        this.disconnect();
    }
    
    @Override
    public void onResume()
    {
        super.onResume();
        //this.connect();
    }
}