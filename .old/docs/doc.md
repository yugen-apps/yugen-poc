# …or create a new repository on the command line
echo "# myRepo" >> README.md
git init
git add README.md
git commit -m "first commit"
git remote add origin https://github.com/emiliano84/myRepo.git
git push -u origin master


# …or push an existing repository from the command line
git remote add origin https://github.com/emiliano84/myRepo.git
git push -u origin master


# forcemerge
git fetch origin   # update all our origin/* remote-tracking branches

git checkout demo         # if needed -- your example assumes you're on it
git merge origin/demo     # if needed -- see below

git checkout master
git merge origin/master

git merge -X theirs demo   # but see below

git push origin master     # again, see below

# There is actually a solution that is much more simple.

Source: https://gist.github.com/cobyism/4730490#gistcomment-2337463

Setup
$ rm -rf dist
$ echo "dist/" >> .gitignore
$ git worktree add dist gh-pages
Making changes
$ make # or what ever you run to populate dist
$ cd dist
$ git add --all
$ git commit -m "$(git log '--format=format:%H' master -1)"
$ git push origin production --force
$ cd ..

# https://sangsoonam.github.io/2019/02/08/using-git-worktree-to-deploy-github-pages.html

Setup
First of all, you need to have a gh-pages branch which is going to be mounted. If you don’t have, you can create a branch with git branch.

$ git branch gh-pages
This makes a branch based on the master HEAD. It would be okay but the files and the git history of master branch are not meaningful on gh-pages branch. Using an orphan branch, you can initialize gh-pages in a clean way.

$ git checkout --orphan gh-pages
$ git reset --hard
$ git commit --allow-empty -m "Init"
$ git checkout master
Then, you can mount the branch as a subdirectory using git worktree. Make sure that you don’t have an existing target subdirectory.

$ rm -rf _site
$ git worktree add _site gh-pages
Preparing _site (identifier _site)
HEAD is now at b475e3e Initializing gh-pages branch
If you didn’t ignore _site in git, ignore it so that you don’t add generated files accidentally.

$ echo "_site" >> .gitignore
When everything is set correctly, git branch shows a different branch for a root directory and a subdirectory.

$ git branch
  gh-pages
* master

$ cd _site
$ git branch
* gh-pages
  master
Deploy
When you build a static site, generated files are in _site directory. Since _site is now gh-pages branch, you can deploy by just creating a commit and pushing it.

$ cd _site
$ git add --all
$ git commit -m "Deploy updates"
$ git push origin gh-pages
Clean Up
To unmount the subdirectory, you can run a below command. When you run, it will remove the worktree information and the mounted subdirectory.

$ git worktree remove _site
If you delete the _site directory without git worktree remove, git worktree doesn’t know it and keep showing it in the git worktree list. It will be eventually removed (gc.worktreePruneExpire). If you clean up immediately, run git worktree prune.

# http://www.hardkoded.com/blog/creating-docfx-site

# https://gist.github.com/cobyism/4730490