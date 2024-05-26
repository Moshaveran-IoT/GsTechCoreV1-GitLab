const { Octokit } = require("@octokit/rest");
const fs = require('fs');

const octokit = new Octokit({
  auth: process.env.GITHUB_TOKEN,
});

(async () => {
  const owner = 'YOUR_GITHUB_USERNAME_OR_ORG';
  const repo = 'YOUR_REPOSITORY_NAME';
  const since = new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString();

  const issues = await octokit.issues.listForRepo({
    owner,
    repo,
    since,
  });

  const report = issues.data.map(issue => ({
    number: issue.number,
    title: issue.title,
    state: issue.state,
    created_at: issue.created_at,
    updated_at: issue.updated_at,
    closed_at: issue.closed_at,
  }));

  fs.writeFileSync('report.txt', JSON.stringify(report, null, 2));

  console.log('Report generated successfully!');
})();
