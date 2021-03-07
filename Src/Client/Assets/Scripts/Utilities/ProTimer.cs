using System;
using System.Diagnostics;
//运行时间计数器
public class ProTimer:IDisposable{
    private string name;
    private int times;
    private Stopwatch watch;
    public ProTimer(string name):this(name,0){

    }

    public ProTimer(string name,int times){
        this.name = name;
        this.times = times;
        if(this.times<=0){
            this.times = 1;
        }
        watch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        watch.Stop();
        float ms = watch.ElapsedMilliseconds;
        if(times>1){
            UnityEngine.Debug.Log(string.Format("ProTimer : [{0}] finished:[{1:0.00}ms] total ,[{2:0.000000}ms] per peroid for [{3}] times",name,ms,ms/times,times));
        }else{
            UnityEngine.Debug.Log(string.Format("ProTimer : [{0}] finished:[{1:0.00}ms] total",name,ms));
        }
    }
}