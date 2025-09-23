<template>
  <footer>
    <div class="links">
      <a
        href="https://github.com/butskristof/partify"
        target="_blank"
        rel="noopener"
      >
        <i
          class="pi pi-github"
          aria-label="GitHub"
        />
      </a>
    </div>
  </footer>
</template>

<script setup lang="ts"></script>

<style scoped lang="scss">
@use '~/assets/styles/utilities.scss';

footer {
  @include utilities.app-container;
  & {
    @include utilities.flex-row-justify-end;
    margin-bottom: calc(var(--default-spacing) * 2);
    padding-inline: var(--default-spacing);
  }

  .links {
    a {
      //cursor: pointer;
      font-size: var(--text-lg);
      color: var(--color-text);
    }
  }
}
</style>
