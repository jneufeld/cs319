﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class Lock
    {
    public int ID;
	[Required]
	public DateTime ExpireTime;
	[Required]
	public Guid UserID;
    [Required]
    public int RequestID;
    }
}