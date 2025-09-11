<template>
  <button class="vote-button">
    <svg
      :class="voted ? 'voted' : ''"
      class="icon"
      width="16px"
      height="14px"
      viewBox="0 0 16 16"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M1.24264 8.24264L8 15L14.7574 8.24264C15.553 7.44699 16 6.36786 16 5.24264V5.05234C16 2.8143 14.1857 1 11.9477 1C10.7166 1 9.55233 1.55959 8.78331 2.52086L8 3.5L7.21669 2.52086C6.44767 1.55959 5.28338 1 4.05234 1C1.8143 1 0 2.8143 0 5.05234V5.24264C0 6.36786 0.44699 7.44699 1.24264 8.24264Z"
      />
    </svg>
    <span
      class="number-of-votes"
      :class="voted ? 'voted' : ''"
      >12</span
    >
  </button>
</template>

<script setup>
defineProps({
  voted: {
    type: Boolean,
    default: false,
  },
});
</script>

<style scoped lang="scss">
.vote-button {
  color: inherit;

  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.25rem;

  padding-inline: 0.5rem;

  //background-color: var(--color-secondary);
  background-color: color-mix(in srgb, var(--color-secondary), transparent 80%);
  border-radius: calc(var(--default-radius) * 2);

  color: var(--color-secondary);
  .number-of-votes {
    //font-size: 80%;

    &.voted {
      font-weight: bold;
    }
  }

  .icon {
    fill: none;
    stroke: var(--color-secondary);
    stroke-width: 2px;

    &.voted {
      fill: var(--color-secondary);
    }
  }
}
</style>
