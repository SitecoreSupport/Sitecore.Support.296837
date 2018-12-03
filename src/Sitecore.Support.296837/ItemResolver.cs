namespace Sitecore.Support.Pipelines.HttpRequest
{
  using Sitecore.Abstractions;
  using Sitecore.Configuration;
  using Sitecore.Data.ItemResolvers;
  using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
  public class ItemResolver : Sitecore.Pipelines.HttpRequest.ItemResolver
  {
    private ItemPathResolver pathResolver;
    [Obsolete("Please use another constructor with parameters")]
    public ItemResolver() : base()
    {
    }
    public ItemResolver(BaseItemManager itemManager, ItemPathResolver pathResolver) : base(itemManager, pathResolver, Settings.ItemResolving.FindBestMatch)
    {
    }

    public override void Process(HttpRequestArgs args)
    {
      Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
      if (!this.SkipItemResolving(args))
      {
        bool permissionDenied = false;
        Item item = null;
        string startPath = string.Empty;
        try
        {
          this.StartProfilingOperation("Resolve current item.", args);
          HashSet<string> set = new HashSet<string>();
          if (args.LocalPath == "/")
          {
            foreach (string str2 in this.GetCandidatePaths(args))
            {
              if (!this.TryResolveItem(str2, args, out item, out permissionDenied))
              {
                break;
              }
              else
              {
                base.Process(args);
                break;
              }
            }
          }
          else
          {
            base.Process(args);
          }
        }
        catch (Exception)
        {
        }
      }
    }
  }
}
