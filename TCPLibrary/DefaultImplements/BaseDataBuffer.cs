﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TCPLibrary.Abstracts;

namespace TCPLibrary.DefaultImplements
{
    /// <summary>
    /// ZDataBuffer的默认实现
    /// </summary>
    public class BaseDataBuffer:ZDataBuffer
    {
        /// <summary>
        /// 按照规定协议，重写TryReadMessage方法
        /// </summary>
        /// <returns></returns>
        internal override ZMessage TryReadMessage()
        {
            if (_length >= 8)   //  4 + 4 + N
            {
                using (MemoryStream ms = new MemoryStream(_buffer))
                {
                    BinaryReader br = new BinaryReader(ms);
                    //int msgtype = br.ReadInt32();  //读取消息类型
                    //int msglength = br.ReadInt32();  //读取消息长度
                    int msglength = 8;
                    if (_length - 0 >= msglength)  //如果缓冲区中存在一条完整消息，则读取
                    {
                        byte[] msgcontent = br.ReadBytes(msglength);  //读取消息内容

                        //byte[] recByte = new byte[4096];
                        //int bytes = br.Read(recByte, 0, msglength);

                        //string recStr = Encoding.ASCII.GetString(recByte, 0, bytes);

                        //byte[] msgcontent = Encoding.ASCII.GetBytes(recStr);


                        BaseMessage bm = new BaseMessage(1, msgcontent); //还原成一条完整的消息
                        Remove(0 + msglength);  //注意！ 移除已读数据

                        return bm;  //返回读取到的消息
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
