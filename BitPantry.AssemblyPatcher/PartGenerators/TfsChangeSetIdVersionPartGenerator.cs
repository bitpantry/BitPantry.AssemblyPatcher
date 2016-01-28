using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BitPantry.AssemblyPatcher.PartGenerators
{

    /// <summary>
    /// A TFS change set version part generator
    /// </summary>
    class TfsChangeSetIdVersionPartGenerator : IVersionPartGenerator
    {
        /// <summary>
        /// Generates a version part which matches the current change set of the working copy being patched
        /// </summary>
        /// <param name="currentPartValue">The current value of the version part - this value is not used by this
        /// generator</param>
        /// <param name="ctx">The version part generation context</param>
        /// <returns>The TFS change set number for the current working copy being patched</returns>
        public int Generate(int currentPartValue, VersionPartPatchingContext ctx)
        {
            // The workspace info for the provided path

            var wsInfo = Workstation.Current.GetLocalWorkspaceInfo(ctx.SolutionRootPath);

            if(wsInfo == null)
                throw new InvalidOperationException(string.Format("The target solution and project must be a TFS working copy - {0}", ctx));

            // Get the TeamProjectCollection and VersionControl server associated with the
            // WorkspaceInfo

            var tpc = new TfsTeamProjectCollection(wsInfo.ServerUri);
            var vcServer = tpc.GetService<VersionControlServer>();

            // Now get the actual Workspace OM object

            var ws = vcServer.GetWorkspace(wsInfo);

            // We are interested in the current version of the workspace

            var versionSpec = new WorkspaceVersionSpec(ws);

            var historyParams = new QueryHistoryParameters(ctx.SolutionRootPath, RecursionType.Full)
            {
                ItemVersion = versionSpec,
                VersionEnd = versionSpec,
                MaxResults = 1
            };

            // return changeset

            var changeset = vcServer.QueryHistory(historyParams).FirstOrDefault();
            if (changeset != null) return changeset.ChangesetId;

            // no changeset found.

            throw new InvalidOperationException(string.Format("Could not find changeset values for solution path \"{0}\"", ctx.SolutionRootPath));
        }



    }
}
