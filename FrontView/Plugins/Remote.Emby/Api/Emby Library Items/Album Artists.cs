using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote.Emby.Api.AlbumArtists
{

    
public class Rootobject
{
public Item[] Items { get; set; }
public int TotalRecordCount { get; set; }
}

public class Item
{
public string Name { get; set; }
public string ServerId { get; set; }
public string Id { get; set; }
public string PlayAccess { get; set; }
public bool IsFolder { get; set; }
public string Type { get; set; }
public Imagetags ImageTags { get; set; }
public string[] BackdropImageTags { get; set; }
public string LocationType { get; set; }
public string ParentBackdropItemId { get; set; }
public string[] ParentBackdropImageTags { get; set; }
public DateTime EndDate { get; set; }
}

public class Imagetags
{
public string Primary { get; set; }
public string Banner { get; set; }
public string Logo { get; set; }
}



}
