-- ============================================================================
-- ENSURE PACKER IS INSTALLED
-- ============================================================================

local fn = vim.fn
local install_path = fn.stdpath('data')..'/site/pack/packer/start/packer.nvim'

if fn.empty(fn.glob(install_path)) > 0 then
  print("Installing packer...")
  fn.system({'git', 'clone', '--depth', '1', 'https://github.com/wbthomason/packer.nvim', install_path})
  vim.cmd [[packadd packer.nvim]]
  print("Packer installed! Please restart Neovim and run :PackerSync")
  return
end

-- Load packer
vim.cmd [[packadd packer.nvim]]

-- ============================================================================
-- PLUGIN INSTALLATION
-- ============================================================================

local status_ok, packer = pcall(require, "packer")
if not status_ok then
  print("Packer failed to load!")
  return
end

packer.startup(function(use)
  -- Packer manages itself
  use 'wbthomason/packer.nvim'
  
  -- Colorschemes
  use 'morhetz/gruvbox'
  use 'folke/tokyonight.nvim'
  use 'catppuccin/nvim'
  
  -- Auto-closing brackets, quotes, etc.
  use {
    'windwp/nvim-autopairs',
    config = function()
      require('nvim-autopairs').setup{}
    end
  }
  
  -- Treesitter for better syntax highlighting
  use {
    'nvim-treesitter/nvim-treesitter',
    run = ':TSUpdate'
  }
  
  -- LSP Configuration
  use 'neovim/nvim-lspconfig'
  use 'williamboman/mason.nvim'
  use 'williamboman/mason-lspconfig.nvim'
  
  -- Autocompletion
  use 'hrsh7th/nvim-cmp'
  use 'hrsh7th/cmp-nvim-lsp'
  use 'hrsh7th/cmp-buffer'
  use 'hrsh7th/cmp-path'
  use 'L3MON4D3/LuaSnip'
  use 'saadparwaiz1/cmp_luasnip'
  
  -- File explorer
  use {
    'nvim-tree/nvim-tree.lua',
    requires = 'nvim-tree/nvim-web-devicons'
  }
  
  -- Status line
  use {
    'nvim-lualine/lualine.nvim',
    requires = 'nvim-tree/nvim-web-devicons'
  }
  
  -- Fuzzy finder
  use {
    'nvim-telescope/telescope.nvim',
    tag = '0.1.5',
    requires = 'nvim-lua/plenary.nvim'
  }
  
  -- Git integration
  use 'lewis6991/gitsigns.nvim'
  
  -- Comment toggling
  use 'numToStr/Comment.nvim'
  
  -- Indent guides
  use 'lukas-reineke/indent-blankline.nvim'
end)

-- ============================================================================
-- BASIC SETTINGS
-- ============================================================================

vim.opt.number = true           -- Show line numbers
vim.opt.relativenumber = true   -- Relative line numbers
vim.opt.expandtab = true        -- Use spaces instead of tabs
vim.opt.shiftwidth = 2          -- Indent size
vim.opt.tabstop = 2             -- Tab size
vim.opt.smartindent = true      -- Auto indent
vim.opt.wrap = false            -- Don't wrap lines
vim.opt.termguicolors = true    -- Enable 24-bit colors
vim.opt.cursorline = true       -- Highlight current line
vim.opt.signcolumn = "yes"      -- Always show sign column
vim.opt.scrolloff = 8           -- Keep 8 lines above/below cursor
vim.opt.updatetime = 250        -- Faster completion
vim.opt.timeoutlen = 300        -- Faster key sequence completion

-- Enable syntax highlighting and filetype detection
vim.cmd("syntax on")
vim.cmd("filetype plugin indent on")

-- Set colorscheme (choose one)
vim.cmd("colorscheme gruvbox")
-- vim.cmd("colorscheme tokyonight-night")
-- vim.cmd("colorscheme catppuccin-mocha")

-- ============================================================================
-- TREESITTER CONFIGURATION
-- ============================================================================

local status_ok, treesitter = pcall(require, 'nvim-treesitter.configs')
if status_ok then
  treesitter.setup {
    -- Install parsers for these languages
    ensure_installed = {
      "c_sharp",
      "typescript",
      "javascript",
      "html",
      "css",
      "scss",
      "json",
      "lua",
      "vim"
    },
    
    -- Install parsers synchronously
    sync_install = false,
    
    -- Automatically install missing parsers
    auto_install = true,
    
    highlight = {
      enable = true,
      additional_vim_regex_highlighting = false,
    },
    
    indent = {
      enable = true
    }
  }
end

-- ============================================================================
-- AUTOPAIRS CONFIGURATION
-- ============================================================================

local status_ok, autopairs = pcall(require, 'nvim-autopairs')
if status_ok then
  autopairs.setup{
    check_ts = true,  -- Use treesitter
    ts_config = {
      lua = {'string'},
      javascript = {'template_string'},
      java = false,
    },
    disable_filetype = { "TelescopePrompt", "vim" },
    fast_wrap = {
      map = '<M-e>',
      chars = { '{', '[', '(', '"', "'" },
      pattern = [=[[%'%"%>%]%)%}%,]]=],
      end_key = '$',
      keys = 'qwertyuiopzxcvbnmasdfghjkl',
      check_comma = true,
      highlight = 'Search',
      highlight_grey='Comment'
    },
  }
end

-- ============================================================================
-- LSP CONFIGURATION (Language Servers)
-- ============================================================================

local status_ok, mason = pcall(require, "mason")
if status_ok then
  mason.setup()
end

local status_ok, mason_lspconfig = pcall(require, "mason-lspconfig")
if status_ok then
  mason_lspconfig.setup({
    ensure_installed = {
      "omnisharp",      -- C#
      "angularls",      -- Angular
      "ts_ls",          -- TypeScript
      "html",           -- HTML
      "cssls",          -- CSS
      "jsonls",         -- JSON
    },
  })
end

-- Setup LSP servers
local status_ok, lspconfig = pcall(require, 'lspconfig')
if status_ok then
  -- C# (OmniSharp)
  lspconfig.omnisharp.setup{
    cmd = { "omnisharp" },
    enable_roslyn_analyzers = true,
    organize_imports_on_format = true,
    enable_import_completion = true,
  }

  -- Angular
  lspconfig.angularls.setup{}

  -- TypeScript
  lspconfig.ts_ls.setup{}

  -- HTML
  lspconfig.html.setup{}

  -- CSS
  lspconfig.cssls.setup{}

  -- JSON
  lspconfig.jsonls.setup{}
end

-- ============================================================================
-- AUTOCOMPLETION SETUP
-- ============================================================================

local status_ok, cmp = pcall(require, 'cmp')
if status_ok then
  local luasnip = require('luasnip')

  cmp.setup({
    snippet = {
      expand = function(args)
        luasnip.lsp_expand(args.body)
      end,
    },
    
    mapping = cmp.mapping.preset.insert({
      ['<C-b>'] = cmp.mapping.scroll_docs(-4),
      ['<C-f>'] = cmp.mapping.scroll_docs(4),
      ['<C-Space>'] = cmp.mapping.complete(),
      ['<C-e>'] = cmp.mapping.abort(),
      ['<CR>'] = cmp.mapping.confirm({ select = true }),
      ['<Tab>'] = cmp.mapping(function(fallback)
        if cmp.visible() then
          cmp.select_next_item()
        elseif luasnip.expand_or_jumpable() then
          luasnip.expand_or_jump()
        else
          fallback()
        end
      end, { 'i', 's' }),
      ['<S-Tab>'] = cmp.mapping(function(fallback)
        if cmp.visible() then
          cmp.select_prev_item()
        elseif luasnip.jumpable(-1) then
          luasnip.jump(-1)
        else
          fallback()
        end
      end, { 'i', 's' }),
    }),
    
    sources = cmp.config.sources({
      { name = 'nvim_lsp' },
      { name = 'luasnip' },
    }, {
      { name = 'buffer' },
      { name = 'path' },
    })
  })
end

-- ============================================================================
-- FILE EXPLORER
-- ============================================================================

local status_ok, nvim_tree = pcall(require, "nvim-tree")
if status_ok then
  nvim_tree.setup()
end

-- ============================================================================
-- STATUS LINE
-- ============================================================================

local status_ok, lualine = pcall(require, 'lualine')
if status_ok then
  lualine.setup {
    options = {
      theme = 'gruvbox',
      component_separators = '|',
      section_separators = '',
    }
  }
end

-- ============================================================================
-- GIT SIGNS
-- ============================================================================

local status_ok, gitsigns = pcall(require, 'gitsigns')
if status_ok then
  gitsigns.setup()
end

-- ============================================================================
-- COMMENT PLUGIN
-- ============================================================================

local status_ok, comment = pcall(require, 'Comment')
if status_ok then
  comment.setup()
end

-- ============================================================================
-- INDENT GUIDES
-- ============================================================================

local status_ok, indent_blankline = pcall(require, "indent_blankline")
if status_ok then
  indent_blankline.setup {
    show_current_context = true,
    show_current_context_start = true,
  }
end

-- ============================================================================
-- KEY MAPPINGS
-- ============================================================================

-- Set leader key
vim.g.mapleader = " "

-- File explorer
vim.keymap.set('n', '<leader>e', ':NvimTreeToggle<CR>', { silent = true })

-- Telescope (fuzzy finder)
vim.keymap.set('n', '<leader>ff', ':Telescope find_files<CR>', { silent = true })
vim.keymap.set('n', '<leader>fg', ':Telescope live_grep<CR>', { silent = true })
vim.keymap.set('n', '<leader>fb', ':Telescope buffers<CR>', { silent = true })

-- LSP keybindings
vim.keymap.set('n', 'gd', vim.lsp.buf.definition, { silent = true })
vim.keymap.set('n', 'K', vim.lsp.buf.hover, { silent = true })
vim.keymap.set('n', 'gi', vim.lsp.buf.implementation, { silent = true })
vim.keymap.set('n', '<leader>rn', vim.lsp.buf.rename, { silent = true })
vim.keymap.set('n', '<leader>ca', vim.lsp.buf.code_action, { silent = true })

-- Window navigation
vim.keymap.set('n', '<C-h>', '<C-w>h')
vim.keymap.set('n', '<C-j>', '<C-w>j')
vim.keymap.set('n', '<C-k>', '<C-w>k')
vim.keymap.set('n', '<C-l>', '<C-w>l')

-- Buffer navigation
vim.keymap.set('n', '<Tab>', ':bnext<CR>', { silent = true })
vim.keymap.set('n', '<S-Tab>', ':bprevious<CR>', { silent = true })

-- Save and quit
vim.keymap.set('n', '<leader>w', ':w<CR>', { silent = true })
vim.keymap.set('n', '<leader>q', ':q<CR>', { silent = true })

print("✅ Neovim configuration loaded successfully!")
